param(
    [Parameter(Mandatory)]
    [ValidateRange(1, 10)]
    [int] $Attempt,

    [string] $ExecutionFile,

    [string] $SessionId
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

function Get-PropertyValue {
    param(
        [AllowNull()]
        [object] $InputObject,

        [Parameter(Mandatory)]
        [string] $Name
    )

    if ($null -eq $InputObject) {
        return $null
    }

    $property = $InputObject.PSObject.Properties[$Name]
    if ($null -eq $property) {
        return $null
    }

    return $property.Value
}

function Write-ClaudeSummary {
    param(
        [Parameter(Mandatory)]
        [hashtable] $Summary
    )

    $retryableCategories = @('transient-provider', 'action-or-bootstrap', 'unreadable-execution')
    $detail = if ($Summary.ContainsKey('error_detail')) { [string] $Summary.error_detail } else { [string] $Summary.detail }
    $retryable = $Summary.category -in $retryableCategories -or
        ($Summary.category -eq 'claude-execution' -and $detail -match '(?i)rate.?limit|overload|timed?.?out|unavailable|capacity|connection|network')
    $Summary.retryable = $retryable

    $json = $Summary | ConvertTo-Json -Compress -Depth 10
    Write-Host "Claude attempt $Attempt execution summary: $json"

    if (-not [string]::IsNullOrWhiteSpace($env:GITHUB_OUTPUT)) {
        Add-Content -LiteralPath $env:GITHUB_OUTPUT -Value "retryable=$($retryable.ToString().ToLowerInvariant())"
    }

    if (-not [string]::IsNullOrWhiteSpace($env:GITHUB_STEP_SUMMARY)) {
        Add-Content -LiteralPath $env:GITHUB_STEP_SUMMARY -Value "### Claude review attempt $Attempt"
        Add-Content -LiteralPath $env:GITHUB_STEP_SUMMARY -Value '```json'
        Add-Content -LiteralPath $env:GITHUB_STEP_SUMMARY -Value $json
        Add-Content -LiteralPath $env:GITHUB_STEP_SUMMARY -Value '```'
    }
}

$summary = @{
    attempt = $Attempt
    session_id = $SessionId
}

if ([string]::IsNullOrWhiteSpace($ExecutionFile) -or -not (Test-Path -LiteralPath $ExecutionFile -PathType Leaf)) {
    $summary.category = 'action-or-bootstrap'
    $summary.detail = 'The action did not provide a readable Claude execution file.'
    Write-ClaudeSummary -Summary $summary
    exit 0
}

try {
    $rawExecution = Get-Content -LiteralPath $ExecutionFile -Raw
    try {
        $events = @($rawExecution | ConvertFrom-Json -Depth 100)
    }
    catch {
        $events = @(
            $rawExecution -split "`r?`n" |
                Where-Object { -not [string]::IsNullOrWhiteSpace($_) } |
                ForEach-Object { $_ | ConvertFrom-Json -Depth 100 }
        )
    }
}
catch {
    $summary.category = 'unreadable-execution'
    $summary.detail = $_.Exception.Message
    Write-ClaudeSummary -Summary $summary
    exit 0
}

$result = $events |
    Where-Object { (Get-PropertyValue -InputObject $_ -Name 'type') -eq 'result' } |
    Select-Object -Last 1

if ($null -eq $result) {
    $summary.category = 'action-or-bootstrap'
    $summary.detail = 'Claude produced no result event.'
    Write-ClaudeSummary -Summary $summary
    exit 0
}

$isError = Get-PropertyValue -InputObject $result -Name 'is_error'
$turns = Get-PropertyValue -InputObject $result -Name 'num_turns'
$cost = Get-PropertyValue -InputObject $result -Name 'total_cost_usd'

$summary.category = if ($isError -eq $true -and $cost -eq 0 -and $turns -le 1) {
    'transient-provider'
}
elseif ($isError -eq $true) {
    'claude-execution'
}
else {
    'action-failure'
}

foreach ($name in @('subtype', 'is_error', 'duration_ms', 'duration_api_ms', 'num_turns', 'total_cost_usd', 'permission_denials_count')) {
    $value = Get-PropertyValue -InputObject $result -Name $name
    if ($null -ne $value) {
        $summary[$name] = $value
    }
}

foreach ($name in @('error', 'errors', 'message', 'result')) {
    $value = Get-PropertyValue -InputObject $result -Name $name
    if ($null -ne $value -and -not [string]::IsNullOrWhiteSpace([string] $value)) {
        $detail = $value | ConvertTo-Json -Compress -Depth 10
        $summary.error_detail = if ($detail.Length -le 1000) { $detail } else { $detail.Substring(0, 1000) + '…' }
        break
    }
}

if (-not $summary.ContainsKey('error_detail') -and $isError -eq $true) {
    $summary.error_detail = 'Provider returned is_error:true without an error field.'
}

Write-ClaudeSummary -Summary $summary
