# EventFlow API Contracts

## Contract-first approach

This directory contains interface definitions maintained independently of
implementation code. Implementations and generated stubs must follow these
contracts; contracts are not reverse-generated from controllers or database
models.

- `openapi.yaml` — public REST API contract for Booking.
- `service.proto` — internal gRPC contract for Booking-to-Inventory communication.
- `generated/java/` — generated Java Protobuf messages and gRPC service stubs.
- `benchmark/` — JSON and Protobuf samples, benchmark script and measured results.
- `tests/test-api.ps1` — REST contract integration checks against Prism.

## REST API

The REST API is intended for browser/mobile clients and external consumers.
It uses OpenAPI 3.0.3, bearer JWT authentication, request correlation through
`X-Request-ID`, and RFC 7807 Problem Details for errors.

Run the mock server from the repository root:

```powershell
npx --yes @stoplight/prism-cli mock api/openapi.yaml --host 127.0.0.1 --port 4010 --errors --verboseLevel info
```

In another PowerShell window:

```powershell
npx --yes @stoplight/spectral-cli lint api/openapi.yaml --ruleset .spectral.yaml
.\api\tests\test-api.ps1
```

The mock server returns examples from the contract. It does not implement
persistent booking state or production business logic.

## gRPC / Protobuf

`service.proto` uses proto3 and defines the internal InventoryService.
Java sources were generated with:

- Protocol Buffers compiler: 36.2
- gRPC Java protoc plugin: 1.85.1
- Java package: `com.eventflow.inventory.v1`

Example generation command (Windows; adjust paths if installed elsewhere):

```powershell
$protoRoot = "$env:LOCALAPPDATA\Microsoft\WinGet\Packages\Google.Protobuf_Microsoft.Winget.Source_8wekyb3d8bbwe"
$protoc = Join-Path $protoRoot "bin\protoc.exe"
$include = Join-Path $protoRoot "include"
$plugin = "$env:TEMP\eventflow-grpc\protoc-gen-grpc-java.exe"

New-Item -ItemType Directory -Force "api\generated\java" | Out-Null

& $protoc `
  --proto_path=api `
  --proto_path="$include" `
  --plugin="protoc-gen-grpc-java=$plugin" `
  --java_out=api/generated/java `
  --grpc-java_out=api/generated/java `
  api/service.proto
```

The generated Java sources are checked in. They are generated source files,
not a compiled or deployed service. A Java build with matching Protobuf and
gRPC runtime dependencies is required to compile and execute them.

Validate the schema:

```powershell
& $protoc --proto_path=api --proto_path="$include" --descriptor_set_out="$env:TEMP\eventflow-descriptor.pb" api/service.proto
```

## Serialization benchmark

Run:

```powershell
py api/benchmark/run_benchmark.py "$protoc" "$include"
```

The script encodes the `ReserveSeatsResponse` text-format message into binary
Protobuf, decodes it again for verification, and records UTF-8 JSON and binary
Protobuf sizes in `benchmark/benchmark-results.json`.

The measured result is specific to the sample payload. It excludes transport,
TLS, HTTP/gRPC framing and compression overhead.
