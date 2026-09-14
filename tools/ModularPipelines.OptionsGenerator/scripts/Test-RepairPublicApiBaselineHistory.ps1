$ErrorActionPreference = 'Stop'
$testRoot = Join-Path ([IO.Path]::GetTempPath()) "public-api-history-$([guid]::NewGuid().ToString('N'))"
[IO.Directory]::CreateDirectory($testRoot) | Out-Null
$repairScript = Join-Path $PSScriptRoot 'Repair-PublicApiBaselineHistory.ps1'
$shippedPath = Join-Path $testRoot 'PublicAPI.Shipped.txt'
$unshippedPath = Join-Path $testRoot 'PublicAPI.Unshipped.txt'

try {
    [IO.File]::WriteAllLines($shippedPath, @('#nullable enable', 'Api.Kept', 'Api.Retired'))
    [IO.File]::WriteAllLines($unshippedPath, @(
        '#nullable enable',
        '*REMOVED*Api.Changed(string)',
        '*REMOVED*Api.Orphan',
        '*REMOVED*Api.Orphan',
        '*REMOVED*Api.Retired',
        'Api.Added',
        'Api.Changed(int)'
    ))

    & $repairScript -PackageDirectory $testRoot
    $expectedShipped = @('#nullable enable', 'Api.Changed(string)', 'Api.Kept', 'Api.Orphan', 'Api.Retired')
    $expectedUnshipped = @(
        '#nullable enable', '*REMOVED*Api.Changed(string)', '*REMOVED*Api.Orphan',
        '*REMOVED*Api.Retired', 'Api.Added', 'Api.Changed(int)'
    )
    foreach ($case in @(
        @{ Path = $shippedPath; Expected = $expectedShipped },
        @{ Path = $unshippedPath; Expected = $expectedUnshipped }
    )) {
        if (-not [Linq.Enumerable]::SequenceEqual(
            [string[]] [IO.File]::ReadAllLines($case.Path), [string[]] $case.Expected, [StringComparer]::Ordinal)) {
            throw "History repair changed the wrong entries in $($case.Path)."
        }
    }

    $before = @((Get-FileHash $shippedPath).Hash, (Get-FileHash $unshippedPath).Hash)
    & $repairScript -PackageDirectory $testRoot
    $after = @((Get-FileHash $shippedPath).Hash, (Get-FileHash $unshippedPath).Hash)
    if (-not [Linq.Enumerable]::SequenceEqual([string[]] $before, [string[]] $after)) {
        throw 'History repair is not idempotent.'
    }

    Write-Output 'OK public API history repair retains removals, signature changes, additions, and valid pairs; duplicate markers are deduplicated.'
}
finally {
    # Only files created by this test are removed; the directory must then be empty.
    Remove-Item -LiteralPath $shippedPath, $unshippedPath -Force -ErrorAction SilentlyContinue
    [IO.Directory]::Delete($testRoot)
}
