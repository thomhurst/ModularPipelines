[CmdletBinding()]
param(
    [Parameter(Mandatory)][string] $PackageDirectory
)

$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot '../../../scripts/PublicApiRemovedMarker.ps1')
$shippedPath = Join-Path $PackageDirectory 'PublicAPI.Shipped.txt'
$unshippedPath = Join-Path $PackageDirectory 'PublicAPI.Unshipped.txt'
$activeApis = [Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
$retiredApis = [Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)

# Reconstruct the recorded active surface without running a compiler or scraping a tool.
# The merge script restores missing shipped entries from these historical markers.
foreach ($path in @($shippedPath, $unshippedPath)) {
    foreach ($line in [IO.File]::ReadAllLines($path)) {
        if (-not (Test-PublicApiEntry $line)) {
            continue
        }

        if (Test-RemovedMarker $line) {
            [void] $retiredApis.Add((Get-RemovedMarkerEntry $line))
        } else {
            [void] $activeApis.Add($line)
        }
    }
}
$activeApis.ExceptWith($retiredApis)

$snapshot = New-TemporaryFile
$removals = New-TemporaryFile
try {
    [IO.File]::WriteAllLines($snapshot.FullName, $activeApis)
    & (Join-Path $PSScriptRoot 'Merge-PublicApiBaselineSnapshot.ps1') `
        -OriginalShippedPath $shippedPath `
        -OriginalUnshippedPath $unshippedPath `
        -CurrentApiSnapshotPath $snapshot.FullName `
        -ConfirmedRemovedApiPath $removals.FullName `
        -ShippedOutputPath $shippedPath `
        -UnshippedOutputPath $unshippedPath
}
finally {
    Remove-Item -LiteralPath $snapshot.FullName, $removals.FullName -Force
}
