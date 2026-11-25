import json
import subprocess
import sys
from pathlib import Path

protoc = sys.argv[1]
include = sys.argv[2]
base = Path("api/benchmark")

json_path = base / "booking.json"
text_path = base / "booking.textproto"
binary_path = base / "booking.pb"

# Invoke protoc directly so stdout remains raw binary bytes.
command = [
    protoc,
    "--proto_path=api",
    f"--proto_path={include}",
    "--encode=eventflow.inventory.v1.ReserveSeatsResponse",
    "api/service.proto",
]

text_proto = text_path.read_bytes()
result = subprocess.run(
    command,
    input=text_proto,
    stdout=subprocess.PIPE,
    stderr=subprocess.PIPE,
)

if result.returncode != 0:
    print(result.stderr.decode(errors="replace"))
    raise SystemExit("Protobuf encoding failed.")

if not result.stdout:
    raise SystemExit("Protobuf output is empty; benchmark aborted.")

binary_path.write_bytes(result.stdout)

# Decode it again to verify that the binary payload is valid.
decode_command = [
    protoc,
    "--proto_path=api",
    f"--proto_path={include}",
    "--decode=eventflow.inventory.v1.ReserveSeatsResponse",
    "api/service.proto",
]
decoded = subprocess.run(
    decode_command,
    input=binary_path.read_bytes(),
    stdout=subprocess.PIPE,
    stderr=subprocess.PIPE,
)

if decoded.returncode != 0:
    print(decoded.stderr.decode(errors="replace"))
    raise SystemExit("Protobuf decode verification failed.")

json_bytes = len(json_path.read_bytes())
proto_bytes = len(binary_path.read_bytes())

if proto_bytes == 0:
    raise SystemExit("Invalid zero-byte Protobuf result.")

saved_bytes = json_bytes - proto_bytes
saved_percent = round(saved_bytes / json_bytes * 100, 2)
ratio = round(json_bytes / proto_bytes, 2)

report = {
    "entity": "ReserveSeatsResponse",
    "json_utf8_bytes": json_bytes,
    "protobuf_binary_bytes": proto_bytes,
    "bytes_saved": saved_bytes,
    "size_reduction_percent": saved_percent,
    "json_to_protobuf_ratio": ratio,
    "protobuf_decode_verified": True,
}

(base / "benchmark-results.json").write_text(
    json.dumps(report, indent=2),
    encoding="utf-8",
)

print("--- VERIFIED PROTOBUF DECODE ---")
print(decoded.stdout.decode())

print("--- BENCHMARK RESULTS ---")
for key, value in report.items():
    print(f"{key}: {value}")

print("--- FILE SIZES ---")
for path in (json_path, text_path, binary_path, base / "benchmark-results.json"):
    print(f"{path}: {path.stat().st_size} bytes")
