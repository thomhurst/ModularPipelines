#!/usr/bin/env pwsh
[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [string]$PackageDirectory,

    [Parameter(Mandatory)]
    [string]$ExpectedVersion,

    [string]$Repository = $env:GITHUB_REPOSITORY,
    [string]$Commit = $env:GITHUB_SHA,
    # The published release's tag: the version, optionally prefixed with v.
    [string]$Tag = "v$ExpectedVersion",
    [switch]$ValidateOnly
)

$ErrorActionPreference = 'Stop'

# SemVer 2.0 without build metadata; each version number must fit AssemblyVersion. The release
# workflow's "Verify release" step and NugetVersionGeneratorModule share this grammar.
function Test-ReleaseVersion([string]$Version) {
    $identifier = '(?:0|[1-9][0-9]*|[0-9]*[A-Za-z-][0-9A-Za-z-]*)'
    $match = [regex]::Match($Version, "\A(0|[1-9][0-9]*)\.(0|[1-9][0-9]*)\.(0|[1-9][0-9]*)(?:-$identifier(?:\.$identifier)*)?\z")
    return $match.Success -and @(1..3 | Where-Object { $match.Groups[$_].Value.Length -gt 5 -or [int]$match.Groups[$_].Value -gt 65534 }).Count -eq 0
}
$manifest = Get-Content -LiteralPath (Join-Path $PackageDirectory 'manifest.json') -Raw | ConvertFrom-Json
if ($manifest.RetrySucceeded -ne $true -or $manifest.PackageCount -lt 1 -or @($manifest.Files).Count -lt 1) {
    throw 'The release requires a successful, nonempty publishing rehearsal.'
}

$files = @($manifest.Files | ForEach-Object { $_.File })
if (@($files | Sort-Object -Unique).Count -ne $files.Count) {
    throw 'The release manifest contains duplicate files.'
}
foreach ($file in $manifest.Files) {
    if ($file.File -notmatch '^[A-Za-z0-9_.+-]+\.(nupkg|snupkg)$' -or $file.Sha256 -notmatch '^[A-Fa-f0-9]{64}$') {
        throw 'The release manifest contains an invalid package name or hash.'
    }
    $hash = (Get-FileHash -LiteralPath (Join-Path $PackageDirectory $file.File) -Algorithm SHA256).Hash
    if ($hash -ne $file.Sha256) {
        throw "Release package '$($file.File)' does not match the validated bytes."
    }
}
$packages = @($files | Where-Object { $_.EndsWith('.nupkg', [StringComparison]::Ordinal) })
if ($packages.Count -ne $manifest.PackageCount) {
    throw 'The release package count does not match the publishing rehearsal.'
}
foreach ($symbols in @($files | Where-Object { $_.EndsWith('.snupkg', [StringComparison]::Ordinal) })) {
    if ([IO.Path]::ChangeExtension($symbols, '.nupkg') -notin $packages) {
        throw "Symbol package '$symbols' has no primary package."
    }
}

# Read the packed core version instead of recomputing a version on a later checkout.
$corePackages = @($packages | Where-Object { $_ -match '^ModularPipelines\.\d' })
if ($corePackages.Count -ne 1) { throw 'The release requires exactly one core package.' }
$archive = [IO.Compression.ZipFile]::OpenRead((Join-Path $PackageDirectory $corePackages[0]))
try {
    $nuspecs = @($archive.Entries | Where-Object { $_.FullName -match '^[^/]+\.nuspec$' })
    if ($nuspecs.Count -ne 1) { throw 'The core package requires exactly one nuspec.' }
    $reader = [IO.StreamReader]::new($nuspecs[0].Open())
    try { [xml]$nuspec = $reader.ReadToEnd() } finally { $reader.Dispose() }
    $metadata = $nuspec.SelectSingleNode('/*[local-name()="package"]/*[local-name()="metadata"]')
    if ($metadata.id -ne 'ModularPipelines' -or
        -not (Test-ReleaseVersion $ExpectedVersion) -or
        $metadata.version.Split('+')[0] -cne $ExpectedVersion.Split('+')[0]) {
        throw 'The packed core version does not match the expected release version.'
    }
}
finally { $archive.Dispose() }

if ($ValidateOnly) {
    Write-Output "Verified $($packages.Count) release packages and their symbols."
    return
}
if ([string]::IsNullOrWhiteSpace($env:NUGET_API_KEY) -or [string]::IsNullOrWhiteSpace($env:GH_TOKEN) -or
    $Repository -notmatch '^[A-Za-z0-9_.-]+/[A-Za-z0-9_.-]+$' -or $Commit -notmatch '^[a-fA-F0-9]{40}$') {
    throw 'Publication requires NuGet and GitHub credentials, a repository, and an exact commit.'
}
if ($Tag -cnotin @("v$ExpectedVersion", $ExpectedVersion)) {
    throw "Release tag '$Tag' does not name version '$ExpectedVersion'."
}

# Publishing the GitHub release created its tag, and this run checked out the tag's commit.
# Query the remote so lightweight and peeled annotated tags are checked before any push.
$tag = $Tag
$tagRef = "refs/tags/$tag"
$remoteTags = @(& git ls-remote --exit-code --tags "https://github.com/$Repository.git" $tagRef "$tagRef^{}")
$tagLookupExitCode = $LASTEXITCODE
if ($tagLookupExitCode -eq 2) { throw "Release tag '$tag' does not exist; publication stopped." }
if ($tagLookupExitCode -ne 0) { throw 'Could not verify the remote release tag; publication stopped.' }
$tagTargets = @{}
foreach ($line in $remoteTags) {
    $parts = $line -split '\s+', 2
    if ($parts.Count -ne 2 -or $parts[0] -notmatch '^[a-fA-F0-9]{40}$' -or
        $parts[1] -notin @($tagRef, "$tagRef^{}") -or $tagTargets.ContainsKey($parts[1])) {
        throw 'The remote release tag lookup returned invalid data.'
    }
    $tagTargets[$parts[1]] = $parts[0]
}
if (-not $tagTargets.ContainsKey($tagRef)) { throw 'The remote release tag lookup returned no tag.' }
$tagCommit = if ($tagTargets.ContainsKey("$tagRef^{}")) { $tagTargets["$tagRef^{}"] } else { $tagTargets[$tagRef] }
if ($tagCommit -ne $Commit) { throw "Release tag '$tag' does not point at the validated commit '$Commit'." }

# Only a published release ships packages: GitHub creates the release, never this script.
& gh api "repos/$Repository/releases?per_page=1" --silent
if ($LASTEXITCODE -ne 0) { throw 'Could not authenticate or access GitHub releases; publication stopped.' }
$releaseState = & gh api "repos/$Repository/releases/tags/$([Uri]::EscapeDataString($tag))" --jq '.draft' 2>$null
if ($LASTEXITCODE -ne 0 -or "$releaseState" -ne 'false') {
    throw "Release '$tag' is not a published GitHub release; publication stopped."
}

# Push symbols explicitly as well: NuGet skips automatic symbols when the primary already exists.
# --skip-duplicate keeps what an earlier attempt pushed, so a failed release can be re-run.
foreach ($file in $files) {
    & dotnet nuget push (Join-Path $PackageDirectory $file) --source 'https://api.nuget.org/v3/index.json' --api-key $env:NUGET_API_KEY --skip-duplicate
    if ($LASTEXITCODE -ne 0) { throw "NuGet upload failed for '$file'; publication stopped." }
}
