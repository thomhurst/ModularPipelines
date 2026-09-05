$ErrorActionPreference = 'Stop'

$mergeScript = Join-Path $PSScriptRoot 'Merge-PublicApiBaselineSnapshot.ps1'
$testRoot = Join-Path ([System.IO.Path]::GetTempPath()) (
    "merge-public-api-baseline-{0}" -f [guid]::NewGuid())
$resolvedTempRoot = [System.IO.Path]::GetFullPath(
    [System.IO.Path]::GetTempPath())
$resolvedTestRoot = [System.IO.Path]::GetFullPath($testRoot)
if (-not $resolvedTestRoot.StartsWith(
        $resolvedTempRoot,
        [System.StringComparison]::OrdinalIgnoreCase)) {
    throw "Refusing to use test directory outside temp root: $resolvedTestRoot"
}

New-Item -ItemType Directory -Path $testRoot | Out-Null

function Assert-Lines([string] $Path, [string[]] $Expected) {
    $actual = @(Get-Content -LiteralPath $Path)
    $expectedLines = @($Expected)
    $isMatch = $actual.Count -eq $expectedLines.Count
    for ($index = 0; $isMatch -and $index -lt $actual.Count; $index++) {
        $isMatch = $actual[$index] -ceq $expectedLines[$index]
    }

    if (-not $isMatch) {
        throw @"
Unexpected contents in $Path.
Expected:
$($expectedLines -join [Environment]::NewLine)
Actual:
$($actual -join [Environment]::NewLine)
"@
    }
}

try {
    $originalShipped = Join-Path $testRoot 'PublicAPI.Shipped.original.txt'
    $originalUnshipped = Join-Path $testRoot 'PublicAPI.Unshipped.original.txt'
    $currentSnapshot = Join-Path $testRoot 'PublicAPI.Current.txt'
    $confirmedRemovals = Join-Path $testRoot 'PublicAPI.ConfirmedRemovals.txt'
    $shippedOutput = Join-Path $testRoot 'PublicAPI.Shipped.txt'
    $unshippedOutput = Join-Path $testRoot 'PublicAPI.Unshipped.txt'

    @(
        '#nullable enable'
        'Api.Changed(string)'
        'Api.Existing'
        'Api.Removed()'
        'Api.SnapshotMissing'
    ) | Set-Content -LiteralPath $originalShipped
    @(
        '#nullable enable'
        '*REMOVED*Api.Historical'
        '*REMOVED*Api.Reintroduced'
        'Api.Pending'
        'Api.Stale'
    ) | Set-Content -LiteralPath $originalUnshipped
    @(
        '#nullable enable'
        'Api.Added'
        'Api.Added.ApiAdded() -> void'
        'Api.Added.Name.get -> string!'
        'Api.Changed(int)'
        'Api.Existing'
        'Api.Pending'
        'Api.Reintroduced'
    ) | Set-Content -LiteralPath $currentSnapshot
    @(
        'Api.Changed(string)'
        'Api.Removed()'
    ) | Set-Content -LiteralPath $confirmedRemovals

    & $mergeScript `
        -OriginalShippedPath $originalShipped `
        -OriginalUnshippedPath $originalUnshipped `
        -CurrentApiSnapshotPath $currentSnapshot `
        -ConfirmedRemovedApiPath $confirmedRemovals `
        -ShippedOutputPath $shippedOutput `
        -UnshippedOutputPath $unshippedOutput

    Assert-Lines $shippedOutput @(
        '#nullable enable'
        'Api.Existing'
        'Api.SnapshotMissing'
    )
    Assert-Lines $unshippedOutput @(
        '#nullable enable'
        '*REMOVED*Api.Changed(string)'
        '*REMOVED*Api.Historical'
        '*REMOVED*Api.Removed()'
        'Api.Added'
        'Api.Added.ApiAdded() -> void'
        'Api.Added.Name.get -> string!'
        'Api.Changed(int)'
        'Api.Pending'
        'Api.Reintroduced'
    )

    $shippedHash = (Get-FileHash -LiteralPath $shippedOutput).Hash
    $unshippedHash = (Get-FileHash -LiteralPath $unshippedOutput).Hash
    & $mergeScript `
        -OriginalShippedPath $shippedOutput `
        -OriginalUnshippedPath $unshippedOutput `
        -CurrentApiSnapshotPath $currentSnapshot `
        -ConfirmedRemovedApiPath $confirmedRemovals `
        -ShippedOutputPath $shippedOutput `
        -UnshippedOutputPath $unshippedOutput

    if ($shippedHash -ne (Get-FileHash -LiteralPath $shippedOutput).Hash -or
        $unshippedHash -ne (Get-FileHash -LiteralPath $unshippedOutput).Hash) {
        throw 'Repeated baseline synchronization was not idempotent.'
    }

    foreach ($path in @(
            $originalShipped,
            $originalUnshipped,
            $currentSnapshot,
            $confirmedRemovals)) {
        [System.IO.File]::WriteAllText($path, '')
    }

    & $mergeScript `
        -OriginalShippedPath $originalShipped `
        -OriginalUnshippedPath $originalUnshipped `
        -CurrentApiSnapshotPath $currentSnapshot `
        -ConfirmedRemovedApiPath $confirmedRemovals `
        -ShippedOutputPath $shippedOutput `
        -UnshippedOutputPath $unshippedOutput

    Assert-Lines $shippedOutput @()
    Assert-Lines $unshippedOutput @()

    Write-Output 'OK public API additions, removals, signature changes, empty inputs, and idempotency passed.'
}
finally {
    if (Test-Path -LiteralPath $resolvedTestRoot) {
        Remove-Item -LiteralPath $resolvedTestRoot -Recurse -Force
    }
}
