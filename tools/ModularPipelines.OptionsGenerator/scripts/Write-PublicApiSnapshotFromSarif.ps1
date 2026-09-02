[CmdletBinding()]
param(
    [Parameter(Mandatory)][string] $ErrorLogPath,
    [Parameter(Mandatory)][string] $SnapshotPath,
    [switch] $RequireEntries
)

$ErrorActionPreference = 'Stop'
$messagePrefix = "Symbol '"
$messageSuffix = "' is not part of the declared public API"
$comparer = [System.StringComparer]::Ordinal

if (-not (Test-Path -LiteralPath $ErrorLogPath -PathType Leaf)) {
    throw "Compiler error log does not exist: $ErrorLogPath"
}

if (-not (Test-Path -LiteralPath $SnapshotPath -PathType Leaf)) {
    throw "Public API snapshot does not exist: $SnapshotPath"
}

$sarif = Get-Content -LiteralPath $ErrorLogPath -Raw | ConvertFrom-Json
$results = @($sarif.runs | ForEach-Object { @($_.results) })
$apis = [System.Collections.Generic.HashSet[string]]::new($comparer)

foreach ($result in $results | Where-Object { $_.ruleId -eq 'RS0016' }) {
    $message = if ($result.message -is [string]) {
        [string] $result.message
    }
    elseif ($null -ne $result.message.text) {
        [string] $result.message.text
    }
    else {
        throw 'RS0016 diagnostic does not contain a supported message.'
    }

    if (-not $message.StartsWith($messagePrefix, [StringComparison]::Ordinal) -or
        -not $message.EndsWith($messageSuffix, [StringComparison]::Ordinal)) {
        throw "Unexpected RS0016 diagnostic message: $message"
    }

    $apiLength = $message.Length - $messagePrefix.Length - $messageSuffix.Length
    if ($apiLength -le 0) {
        throw "RS0016 diagnostic contains an empty symbol: $message"
    }

    $null = $apis.Add($message.Substring($messagePrefix.Length, $apiLength))
}

if ($RequireEntries -and $apis.Count -eq 0) {
    throw 'Compiler error log contains no RS0016 public API entries.'
}

$headers = @(Get-Content -LiteralPath $SnapshotPath | Where-Object {
    $_.StartsWith('#', [StringComparison]::Ordinal)
})
$sortedApis = [string[]] @($apis)
[Array]::Sort($sortedApis, $comparer)
[IO.File]::WriteAllLines(
    $SnapshotPath,
    [string[]] (@($headers) + $sortedApis),
    [Text.UTF8Encoding]::new($false))

Write-Output "Wrote $($apis.Count) public API entries from compiler diagnostics."
