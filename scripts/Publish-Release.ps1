[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [string]$PackageDirectory,

    [Parameter(Mandatory)]
    [string]$ExpectedVersion,

    [string]$Repository = $env:GITHUB_REPOSITORY,
    [string]$Commit = $env:GITHUB_SHA,
    [switch]$ValidateOnly
)

$ErrorActionPreference = 'Stop'
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
        $ExpectedVersion -notmatch '^\d+\.\d+\.\d+(?:-[0-9A-Za-z.-]+)?(?:\+[0-9A-Za-z.-]+)?$' -or
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

# Push symbols explicitly as well: NuGet skips automatic symbols when the primary already exists.
foreach ($file in $files) {
    & dotnet nuget push (Join-Path $PackageDirectory $file) --source 'https://api.nuget.org/v3/index.json' --api-key $env:NUGET_API_KEY --skip-duplicate
    if ($LASTEXITCODE -ne 0) { throw "NuGet upload failed for '$file'; release creation stopped." }
}

$tag = "v$ExpectedVersion"
& gh release view $tag --repo $Repository *> $null
if ($LASTEXITCODE -ne 0) {
    $releaseArguments = @('release', 'create', $tag, '--repo', $Repository, '--target', $Commit,
        '--title', $ExpectedVersion, '--generate-notes')
    if ($ExpectedVersion.Split('+')[0].Contains('-')) { $releaseArguments += '--prerelease' }
    & gh @releaseArguments
    if ($LASTEXITCODE -ne 0) { throw 'GitHub release creation failed.' }
}
