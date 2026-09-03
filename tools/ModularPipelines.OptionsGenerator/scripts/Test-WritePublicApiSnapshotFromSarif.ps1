$ErrorActionPreference = 'Stop'

$snapshotScript = Join-Path $PSScriptRoot 'Write-PublicApiSnapshotFromSarif.ps1'
$testRoot = Join-Path ([IO.Path]::GetTempPath()) (
    "write-public-api-snapshot-{0}" -f [guid]::NewGuid())
$resolvedTempRoot = [IO.Path]::GetFullPath([IO.Path]::GetTempPath())
$resolvedTestRoot = [IO.Path]::GetFullPath($testRoot)
if (-not $resolvedTestRoot.StartsWith(
        $resolvedTempRoot,
        [StringComparison]::OrdinalIgnoreCase)) {
    throw "Refusing to use test directory outside temp root: $resolvedTestRoot"
}

New-Item -ItemType Directory -Path $resolvedTestRoot | Out-Null
$packageDirectory = Join-Path $resolvedTestRoot 'src/Target.Package'
$foreignDirectory = Join-Path $resolvedTestRoot 'src/Foreign.Package'
New-Item -ItemType Directory -Path $packageDirectory, $foreignDirectory | Out-Null
$targetUri = [Uri]::new((Join-Path $packageDirectory 'Target.cs')).AbsoluteUri
$foreignUri = [Uri]::new((Join-Path $foreignDirectory 'Foreign.cs')).AbsoluteUri

function Write-Sarif([string] $Path, [object[]] $Results) {
    foreach ($result in $Results) {
        if (-not $result.ContainsKey('locations')) {
            $result.locations = @(@{
                physicalLocation = @{ artifactLocation = @{ uri = $targetUri } }
            })
        }
    }

    @{
        version = '2.1.0'
        runs = @(@{ results = $Results })
    } | ConvertTo-Json -Depth 10 | Set-Content -LiteralPath $Path
}

function Write-SarifV1([string] $Path, [object[]] $Results) {
    foreach ($result in $Results) {
        $result.locations = @(@{ resultFile = @{ uri = $targetUri } })
    }

    @{
        version = '1.0.0'
        runs = @(@{ results = $Results })
    } | ConvertTo-Json -Depth 10 | Set-Content -LiteralPath $Path
}

try {
    $errorLog = Join-Path $resolvedTestRoot 'compiler.sarif'
    $snapshot = Join-Path $resolvedTestRoot 'PublicAPI.Unshipped.txt'
    @('#nullable enable', 'stale') | Set-Content -LiteralPath $snapshot
    Write-Sarif $errorLog @(
        @{ ruleId = 'CS1591'; message = "Missing XML comment" }
        @{ ruleId = 'RS0016'; message = "Symbol 'api.Lower' is not part of the declared public API" }
        @{ ruleId = 'RS0016'; message = "Symbol 'Api.Upper' is not part of the declared public API" }
        @{ ruleId = 'RS0016'; message = "Symbol 'Api.Upper' is not part of the declared public API" }
        @{
            ruleId = 'RS0016'
            message = "Symbol 'Foreign.Api' is not part of the declared public API"
            locations = @(@{
                physicalLocation = @{ artifactLocation = @{ uri = $foreignUri } }
            })
        }
    )

    & $snapshotScript `
        -ErrorLogPath $errorLog `
        -SnapshotPath $snapshot `
        -PackageDirectory $packageDirectory `
        -RequireEntries

    $actual = @(Get-Content -LiteralPath $snapshot)
    $expected = @('#nullable enable', 'Api.Upper', 'api.Lower')
    if (-not [Linq.Enumerable]::SequenceEqual(
            [string[]] $actual,
            [string[]] $expected,
            [StringComparer]::Ordinal)) {
        throw "Unexpected snapshot contents: $($actual -join ', ')"
    }

    Write-SarifV1 $errorLog @(
        @{ ruleId = 'RS0016'; message = "Symbol 'Api.FromSarifV1' is not part of the declared public API" }
    )
    & $snapshotScript `
        -ErrorLogPath $errorLog `
        -SnapshotPath $snapshot `
        -PackageDirectory $packageDirectory `
        -RequireEntries
    if ((Get-Content -LiteralPath $snapshot -Raw) -notmatch 'Api\.FromSarifV1') {
        throw 'SARIF v1 resultFile URI was not extracted.'
    }

    Write-Sarif $errorLog @(
        @{ ruleId = 'RS0016'; message = 'Unexpected message shape' }
    )
    try {
        & $snapshotScript `
            -ErrorLogPath $errorLog `
            -SnapshotPath $snapshot `
            -PackageDirectory $packageDirectory
        throw 'Malformed RS0016 diagnostic should have failed.'
    }
    catch {
        if ($_.Exception.Message -eq 'Malformed RS0016 diagnostic should have failed.') {
            throw
        }
    }

    Write-Sarif $errorLog @()
    try {
        & $snapshotScript `
            -ErrorLogPath $errorLog `
            -SnapshotPath $snapshot `
            -PackageDirectory $packageDirectory `
            -RequireEntries
        throw 'Empty required snapshot should have failed.'
    }
    catch {
        if ($_.Exception.Message -eq 'Empty required snapshot should have failed.') {
            throw
        }
    }

    Write-Output 'OK SARIF extraction, deduplication, ordering, and fail-closed checks passed.'
}
finally {
    if (Test-Path -LiteralPath $resolvedTestRoot) {
        Remove-Item -LiteralPath $resolvedTestRoot -Recurse -Force
    }
}
