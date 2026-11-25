$ErrorActionPreference = "Stop"

$baseUrl = "http://127.0.0.1:4010"
$headers = @{
    Authorization = "Bearer demo-token"
    "X-Request-ID" = "eventflow-integration-test"
}

$validBody = @{
    eventId = "7a1b2c3d-4e5f-6789-abcd-ef0123456789"
    seatIds = @("11111111-1111-4111-8111-111111111111")
    promoCode = "EARLYBIRD"
} | ConvertTo-Json

$invalidBody = @{
    eventId = "not-a-uuid"
    seatIds = @()
} | ConvertTo-Json

$tests = @(
    @{
        Name = "Valid booking POST"
        Method = "POST"
        Uri = "$baseUrl/v1/bookings"
        Expected = 201
        Body = $validBody
    },
    @{
        Name = "Invalid booking payload"
        Method = "POST"
        Uri = "$baseUrl/v1/bookings"
        Expected = 422
        Body = $invalidBody
    },
    @{
        Name = "Filtered and paginated GET"
        Method = "GET"
        Uri = "$baseUrl/v1/bookings?limit=10&status=CONFIRMED&cursor=demo-cursor"
        Expected = 200
    },
    @{
        Name = "Limit above maximum"
        Method = "GET"
        Uri = "$baseUrl/v1/bookings?limit=101"
        Expected = 422
    }
)

$results = foreach ($test in $tests) {
    $params = @{
        Uri = $test.Uri
        Method = $test.Method
        Headers = $headers
        UseBasicParsing = $true
        ErrorAction = "Stop"
    }

    if ($test.Body) {
        $params.ContentType = "application/json"
        $params.Body = $test.Body
    }

    $actual = 0
    try {
        $response = Invoke-WebRequest @params
        $actual = [int]$response.StatusCode
    }
    catch {
        if ($_.Exception.Response) {
            $actual = [int]$_.Exception.Response.StatusCode
        } else {
            throw
        }
    }

    [PSCustomObject]@{
        Test = $test.Name
        Expected = $test.Expected
        Actual = $actual
        Result = if ($actual -eq $test.Expected) { "PASS" } else { "FAIL" }
    }
}

$results | Format-Table -AutoSize

$failed = @($results | Where-Object { $_.Result -eq "FAIL" }).Count
if ($failed -gt 0) {
    throw "$failed test(s) failed."
}

Write-Host "SUCCESS: all REST integration tests passed." -ForegroundColor Green
