[CmdletBinding()]
param(
    [Parameter(Mandatory)][string] $ErrorLogPath,
    [Parameter(Mandatory)][string] $SnapshotPath,
    [Parameter(Mandatory)][string] $PackageDirectory
)

$ErrorActionPreference = 'Stop'
$messagePrefix = "Symbol '"
$messageSuffix = "' is part of the declared API, but is either not public or could not be found"
$comparer = [System.StringComparer]::Ordinal

$results = @(& (Join-Path $PSScriptRoot 'Get-ProjectSarifResults.ps1') `
    -ErrorLogPath $ErrorLogPath `
    -PackageDirectory $PackageDirectory `
    -RuleId RS0017)
$removedApis = [System.Collections.Generic.HashSet[string]]::new($comparer)

foreach ($result in $results) {
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
