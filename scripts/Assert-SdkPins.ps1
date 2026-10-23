[CmdletBinding()]
param(
    [string]$RepositoryRoot = (Split-Path $PSScriptRoot -Parent)
)

$ErrorActionPreference = 'Stop'
$sdkVersion = (Get-Content -LiteralPath (Join-Path $RepositoryRoot 'global.json') -Raw | ConvertFrom-Json).sdk.version
if ($sdkVersion -notmatch '^\d+\.\d+\.\d+(?:-[0-9A-Za-z][0-9A-Za-z.-]*)?$') {
    throw 'global.json must pin a valid SDK version.'
}
if ($sdkVersion.Contains('-')) {
    Write-Host "Root SDK $sdkVersion is prerelease; GA pin validation will apply after the SDK switch."
    return
}

# Keep this list aligned with the GA switch checklist in #3997. The optional tool
# manifest is checked if restored, so removing the unused tool cannot hide drift.
$pinFiles = @(
    'global.json',
    'test/ModularPipelines.TestsForTests/global.json',
    'src/ModularPipelines.Analyzers/ModularPipelines.Analyzers.Test/Net.cs'
)
$toolManifest = '.config/dotnet-tools.json'
if (Test-Path -LiteralPath (Join-Path $RepositoryRoot $toolManifest)) {
    $pinFiles += $toolManifest
}

$failures = foreach ($relativePath in $pinFiles) {
    $path = Join-Path $RepositoryRoot $relativePath
    if (-not (Test-Path -LiteralPath $path -PathType Leaf)) {
        "${relativePath}: required SDK pin file is missing."
        continue
    }
    foreach ($match in Select-String -LiteralPath $path -Pattern '"\d+\.\d+\.\d+-[0-9A-Za-z][0-9A-Za-z.-]*"') {
        "${relativePath}:$($match.LineNumber): $($match.Line.Trim())"
    }
}
if ($failures) {
    throw "Root SDK $sdkVersion is stable. Remove remaining prerelease pins in the same GA switch (see #3997):`n$($failures -join "`n")"
}
Write-Host "SDK pin validation passed for stable root SDK $sdkVersion."
