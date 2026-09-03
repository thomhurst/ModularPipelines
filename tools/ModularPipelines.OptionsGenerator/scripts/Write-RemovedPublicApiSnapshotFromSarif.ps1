[CmdletBinding()]
param(
    [Parameter(Mandatory)][string] $ErrorLogPath,
    [Parameter(Mandatory)][string] $SnapshotPath
)

$ErrorActionPreference = 'Stop'
$messagePrefix = "Symbol '"
$messageSuffix = "' is part of the declared API, but is either not public or could not be found"
$comparer = [System.StringComparer]::Ordinal

if (-not (Test-Path -LiteralPath $ErrorLogPath -PathType Leaf)) {
    throw "Compiler error log does not exist: $ErrorLogPath"
}

$sarif = Get-Content -LiteralPath $ErrorLogPath -Raw | ConvertFrom-Json
$results = @($sarif.runs | ForEach-Object { @($_.results) })
$removedApis = [System.Collections.Generic.HashSet[string]]::new($comparer)

foreach ($result in $results | Where-Object { $_.ruleId -eq 'RS0017' }) {
    $message = if ($result.message -is [string]) {
        [string] $result.message
    }
    elseif ($null -ne $result.message.text) {
        [string] $result.message.text
    }
    else {
        throw 'RS0017 diagnostic does not contain a supported message.'
    }

    if (-not $message.StartsWith($messagePrefix, [StringComparison]::Ordinal) -or
        -not $message.EndsWith($messageSuffix, [StringComparison]::Ordinal)) {
        throw "Unexpected RS0017 diagnostic message: $message"
    }

    $apiLength = $message.Length - $messagePrefix.Length - $messageSuffix.Length
    if ($apiLength -le 0) {
        throw "RS0017 diagnostic contains an empty symbol: $message"
    }

    $null = $removedApis.Add($message.Substring($messagePrefix.Length, $apiLength))
}

$sortedApis = [string[]] @($removedApis)
[Array]::Sort($sortedApis, $comparer)
[IO.File]::WriteAllLines(
    $SnapshotPath,
    $sortedApis,
    [Text.UTF8Encoding]::new($false))

Write-Output "Wrote $($removedApis.Count) confirmed public API removals from compiler diagnostics."
