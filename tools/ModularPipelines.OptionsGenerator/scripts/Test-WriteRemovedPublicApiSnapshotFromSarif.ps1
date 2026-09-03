$ErrorActionPreference = 'Stop'

$snapshotScript = Join-Path $PSScriptRoot 'Write-RemovedPublicApiSnapshotFromSarif.ps1'
$testRoot = Join-Path ([IO.Path]::GetTempPath()) (
    "write-removed-public-api-snapshot-{0}" -f [guid]::NewGuid())
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

try {
    $errorLog = Join-Path $resolvedTestRoot 'compiler.sarif'
    $snapshot = Join-Path $resolvedTestRoot 'PublicAPI.ConfirmedRemovals.txt'
    Write-Sarif $errorLog @(
        @{ ruleId = 'RS0016'; message = "Symbol 'Api.Added' is not part of the declared public API" }
        @{
            ruleId = 'RS0017'
            message = @{ text = "Symbol 'api.Lower' is part of the declared API, but is either not public or could not be found" }
        }
        @{ ruleId = 'RS0017'; message = "Symbol 'Api.Upper' is part of the declared API, but is either not public or could not be found" }
        @{ ruleId = 'RS0017'; message = "Symbol 'Api.Upper' is part of the declared API, but is either not public or could not be found" }
        @{
            ruleId = 'RS0017'
            message = "Symbol 'Foreign.Api' is part of the declared API, but is either not public or could not be found"
            locations = @(@{
                physicalLocation = @{ artifactLocation = @{ uri = $foreignUri } }
            })
        }
    )

    & $snapshotScript `
        -ErrorLogPath $errorLog `
        -SnapshotPath $snapshot `
        -PackageDirectory $packageDirectory

    $actual = @(Get-Content -LiteralPath $snapshot)
    $expected = @('Api.Upper', 'api.Lower')
    if (-not [Linq.Enumerable]::SequenceEqual(
            [string[]] $actual,
            [string[]] $expected,
            [StringComparer]::Ordinal)) {
        throw "Unexpected removal snapshot contents: $($actual -join ', ')"
    }

    Write-Sarif $errorLog @(
        @{ ruleId = 'RS0017'; message = 'Unexpected message shape' }
    )
    try {
        & $snapshotScript `
            -ErrorLogPath $errorLog `
            -SnapshotPath $snapshot `
            -PackageDirectory $packageDirectory
        throw 'Malformed RS0017 diagnostic should have failed.'
    }
    catch {
        if ($_.Exception.Message -eq 'Malformed RS0017 diagnostic should have failed.') {
            throw
        }
    }

    Write-Sarif $errorLog @()
    & $snapshotScript `
        -ErrorLogPath $errorLog `
        -SnapshotPath $snapshot `
        -PackageDirectory $packageDirectory
    if ((Get-Item -LiteralPath $snapshot).Length -ne 0) {
        throw 'Empty removal evidence should produce an empty snapshot.'
    }

    Write-Output 'OK RS0017 extraction, deduplication, ordering, and validation passed.'
}
finally {
    if (Test-Path -LiteralPath $resolvedTestRoot) {
        Remove-Item -LiteralPath $resolvedTestRoot -Recurse -Force
    }
}
