$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$scriptPath = Join-Path $PSScriptRoot 'Summarize-ClaudeExecution.ps1'
$testDirectory = Join-Path ([System.IO.Path]::GetTempPath()) "claude-execution-summary-$([guid]::NewGuid())"

function Assert-Contains {
    param(
        [Parameter(Mandatory)]
        [string] $Actual,

        [Parameter(Mandatory)]
        [string] $Expected
    )

    if (-not $Actual.Contains($Expected, [System.StringComparison]::Ordinal)) {
        throw "Expected output to contain '$Expected'. Actual output: $Actual"
    }
}

New-Item -ItemType Directory -Path $testDirectory | Out-Null

try {
    $transientPath = Join-Path $testDirectory 'transient.jsonl'
    @'
{"type":"system","subtype":"init","model":"claude-sonnet-5"}
{"type":"result","subtype":"success","is_error":true,"duration_ms":740,"num_turns":1,"total_cost_usd":0}
'@ | Set-Content -LiteralPath $transientPath

    $output = & $scriptPath -Attempt 1 -ExecutionFile $transientPath -SessionId 'session-1' *>&1 | Out-String
    Assert-Contains -Actual $output -Expected '"category":"transient-provider"'
    Assert-Contains -Actual $output -Expected '"retryable":true'
    Assert-Contains -Actual $output -Expected '"session_id":"session-1"'
    Assert-Contains -Actual $output -Expected 'Provider returned is_error:true without an error field.'

    $errorPath = Join-Path $testDirectory 'error.json'
    @(
        [ordered]@{
            type = 'result'
            subtype = 'error_during_execution'
            is_error = $true
            num_turns = 2
            total_cost_usd = 0.01
            error = 'Rate limit exceeded'
        }
    ) | ConvertTo-Json | Set-Content -LiteralPath $errorPath

    $output = & $scriptPath -Attempt 2 -ExecutionFile $errorPath -SessionId 'session-2' *>&1 | Out-String
    Assert-Contains -Actual $output -Expected '"category":"claude-execution"'
    Assert-Contains -Actual $output -Expected '"retryable":true'
    Assert-Contains -Actual $output -Expected 'Rate limit exceeded'

    $nonRetryablePath = Join-Path $testDirectory 'non-retryable.json'
    [ordered]@{
        type = 'result'
        subtype = 'error_during_execution'
        is_error = $true
        num_turns = 2
        total_cost_usd = 0.01
        error = 'Invalid authentication credentials'
    } | ConvertTo-Json | Set-Content -LiteralPath $nonRetryablePath

    $output = & $scriptPath -Attempt 1 -ExecutionFile $nonRetryablePath *>&1 | Out-String
    Assert-Contains -Actual $output -Expected '"retryable":false'

    $output = & $scriptPath -Attempt 1 -ExecutionFile (Join-Path $testDirectory 'missing.json') *>&1 | Out-String
    Assert-Contains -Actual $output -Expected '"category":"action-or-bootstrap"'

    Write-Host 'All Claude execution summary tests passed.'
}
finally {
    Remove-Item -LiteralPath $testDirectory -Recurse -Force
}
