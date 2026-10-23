$ErrorActionPreference = 'Stop'
$repositoryRoot = Split-Path $PSScriptRoot -Parent
$guard = Join-Path $PSScriptRoot 'Assert-SdkPins.ps1'
$fixture = Join-Path $repositoryRoot "artifacts/sdk-pins-$([guid]::NewGuid().ToString('N'))"
$nestedGlobal = 'test/ModularPipelines.TestsForTests/global.json'
$analyzerReferences = 'src/ModularPipelines.Analyzers/ModularPipelines.Analyzers.Test/Net.cs'
$toolManifest = '.config/dotnet-tools.json'

function Write-FixtureFile([string]$Path, [string]$Content) {
    $destination = Join-Path $fixture $Path
    New-Item -ItemType Directory -Path (Split-Path $destination -Parent) -Force | Out-Null
    Set-Content -LiteralPath $destination -Value $Content
}

function Assert-Rejected([string]$ExpectedPath) {
    $failure = $null
    try { & $guard -RepositoryRoot $fixture 6>$null }
    catch { $failure = $_.Exception.Message }
    if (-not $failure -or -not $failure.Contains($ExpectedPath)) {
        throw "Expected rejection naming '$ExpectedPath', received '$failure'."
    }
}

try {
    # The current RC configuration remains usable until the GA switch.
    Write-FixtureFile 'global.json' '{"sdk":{"version":"11.0.100-rc.1.26425.128"}}'
    Write-FixtureFile $nestedGlobal '{"sdk":{"version":"11.0.100-rc.1.26425.128"}}'
    Write-FixtureFile $analyzerReferences 'new PackageIdentity("Microsoft.NETCore.App.Ref", "11.0.0-rc.1.26425.128")'
    & $guard -RepositoryRoot $fixture

    # Moving only the root SDK to GA must fail on both remaining locations.
    Write-FixtureFile 'global.json' '{"sdk":{"version":"11.0.100"}}'
    Assert-Rejected $nestedGlobal
    Assert-Rejected $analyzerReferences
    Write-FixtureFile $nestedGlobal '{"sdk":{"version":"11.0.100"}}'
    Assert-Rejected $analyzerReferences
    Write-FixtureFile $analyzerReferences 'new PackageIdentity("Microsoft.NETCore.App.Ref", "11.0.0")'
    & $guard -RepositoryRoot $fixture

    # Check either analyzer package (including previews), not just the reference pack.
    Write-FixtureFile $analyzerReferences 'new PackageIdentity("Microsoft.NETCore.App.Ref", "11.0.0"); new PackageIdentity("Microsoft.Extensions.Logging", "11.0.0-preview.7")'
    Assert-Rejected $analyzerReferences
    Write-FixtureFile $analyzerReferences 'new PackageIdentity("Microsoft.NETCore.App.Ref", "11.0.0"); new PackageIdentity("Microsoft.Extensions.Logging", "11.0.0")'

    # The unused tool can be removed, but a restored RC tool pin must be rejected.
    Write-FixtureFile $toolManifest '{"tools":{"microsoft.dotnet.apicompat.tool":{"version":"11.0.100-rc.2.26504.105"}}}'
    Assert-Rejected $toolManifest
    Write-FixtureFile $toolManifest '{"tools":{"microsoft.dotnet.apicompat.tool":{"version":"11.0.100"}}}'
    & $guard -RepositoryRoot $fixture
    Remove-Item -LiteralPath (Join-Path $fixture $toolManifest)
    & $guard -RepositoryRoot $fixture

    # Missing required files or an invalid root SDK must fail closed.
    Remove-Item -LiteralPath (Join-Path $fixture $nestedGlobal)
    Assert-Rejected $nestedGlobal
    Write-FixtureFile 'global.json' '{"sdk":{}}'
    Assert-Rejected 'global.json'
    Write-Host 'SDK pin regression tests passed.'
}
finally {
    $artifactRoot = [IO.Path]::GetFullPath((Join-Path $repositoryRoot 'artifacts')) + [IO.Path]::DirectorySeparatorChar
    if (-not [IO.Path]::GetFullPath($fixture).StartsWith($artifactRoot, [StringComparison]::OrdinalIgnoreCase)) {
        throw "Refusing to remove fixture outside '$artifactRoot'."
    }
    if (Test-Path -LiteralPath $fixture) { Remove-Item -LiteralPath $fixture -Recurse -Force }
}
