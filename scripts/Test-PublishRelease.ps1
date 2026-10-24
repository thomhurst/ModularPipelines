$ErrorActionPreference = 'Stop'
$repositoryRoot = Split-Path $PSScriptRoot -Parent
$publishScript = Join-Path $PSScriptRoot 'Publish-Release.ps1'
$directory = Join-Path $repositoryRoot "artifacts/publish-release-tests/$([guid]::NewGuid().ToString('N'))"
$originalNugetKey = $env:NUGET_API_KEY
$originalGitHubToken = $env:GH_TOKEN
$originalExitCode = $global:LASTEXITCODE
$calls = [Collections.Generic.List[object]]::new()
$failUpload = $false
$failRelease = $false
$releaseExists = $false
function dotnet {
    $calls.Add(@($args))
    $global:LASTEXITCODE = if ($failUpload) { 1 } else { 0 }
}
function gh {
    $calls.Add(@($args))
    $global:LASTEXITCODE = if (($args[1] -eq 'view' -and -not $releaseExists) -or
        ($args[1] -eq 'create' -and $failRelease)) { 1 } else { 0 }
}

function Write-Fixture([string]$Version = '4.2.3') {
    $package = Join-Path $directory "ModularPipelines.$Version.nupkg"
    if (Test-Path $package) { Remove-Item -LiteralPath $package }
    $archive = [IO.Compression.ZipFile]::Open($package, [IO.Compression.ZipArchiveMode]::Create)
    try {
        $writer = [IO.StreamWriter]::new($archive.CreateEntry('ModularPipelines.nuspec').Open())
        try { $writer.Write("<package xmlns='http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd'><metadata><id>ModularPipelines</id><version>$Version</version></metadata></package>") }
        finally { $writer.Dispose() }
    }
    finally { $archive.Dispose() }
    $symbols = [IO.Path]::ChangeExtension($package, '.snupkg')
    Set-Content -LiteralPath $symbols -Value 'symbols fixture'
    $files = @($package, $symbols | ForEach-Object {
        @{ File = [IO.Path]::GetFileName($_); Sha256 = (Get-FileHash -LiteralPath $_ -Algorithm SHA256).Hash }
    })
    @{ PackageCount = 1; RetrySucceeded = $true; Files = $files } |
        ConvertTo-Json -Depth 5 | Set-Content -LiteralPath (Join-Path $directory 'manifest.json')
    $calls.Clear()
}

function Assert-Rejected([scriptblock]$Action, [string]$Reason) {
    $failed = $false
    try { & $Action } catch { $failed = $true }
    if (-not $failed -or $calls.Count -ne 0) { throw "Expected rejection before any external command: $Reason" }
}

try {
    New-Item -ItemType Directory -Path $directory -Force | Out-Null
    $env:NUGET_API_KEY = 'fixture-key'
    $env:GH_TOKEN = 'fixture-token'
    $parameters = @{ PackageDirectory = $directory; ExpectedVersion = '4.2.3'; Repository = 'owner/repo'; Commit = 'a' * 40 }
    foreach ($version in @('4.2.3', '4.2.3-alpha0012', '4.2.3+build.1')) {
        Write-Fixture $version
        $parameters.ExpectedVersion = $version
        & $publishScript @parameters -ValidateOnly | Out-Null
        if ($calls.Count -ne 0) { throw 'Validation must not authenticate or publish.' }
        & $publishScript @parameters
        if ($calls.Count -ne 4 -or $calls[0][0] -ne 'nuget' -or $calls[1][0] -ne 'nuget' -or
            $calls[0] -notcontains '--skip-duplicate' -or $calls[1] -notcontains '--skip-duplicate' -or
            $calls[1][2] -notlike '*.snupkg' -or $calls[2][1] -ne 'view' -or $calls[3][1] -ne 'create' -or
            $calls[3][2] -ne "v$version" -or $calls[3] -notcontains '--generate-notes' -or
            ($calls[3] -contains '--prerelease') -ne $version.Split('+')[0].Contains('-') -or
            $calls[3][6] -ne ('a' * 40)) {
            throw "Publication must upload the verified packages/symbols before creating release $version at the validated commit."
        }
    }
    $parameters.ExpectedVersion = '4.2.3'
    Write-Fixture
    $releaseExists = $true
    & $publishScript @parameters
    if ($calls.Count -ne 3) { throw 'Retry must keep an existing release.' }
    $releaseExists = $false

    Write-Fixture
    $failUpload = $true
    $failed = $false
    try { & $publishScript @parameters } catch { $failed = $true }
    if (-not $failed -or $calls.Count -ne 1) { throw 'An upload failure must stop subsequent pushes and release creation.' }
    $failUpload = $false

    Write-Fixture
    $failRelease = $true
    $failed = $false
    try { & $publishScript @parameters } catch { $failed = $true }
    if (-not $failed -or $calls.Count -ne 4) { throw 'Release creation failures must fail publication.' }
    $failRelease = $false

    Write-Fixture
    $manifestPath = Join-Path $directory 'manifest.json'
    $manifest = Get-Content -LiteralPath $manifestPath -Raw | ConvertFrom-Json
    $contentPackage = Join-Path $directory 'ModularPipelines.Templates.4.2.3.nupkg'
    Set-Content -LiteralPath $contentPackage -Value 'content-only package fixture'
    $manifest.PackageCount = 2
    $manifest.Files += @{ File = [IO.Path]::GetFileName($contentPackage); Sha256 = (Get-FileHash -LiteralPath $contentPackage).Hash }
    $manifest | ConvertTo-Json -Depth 5 | Set-Content -LiteralPath $manifestPath
    & $publishScript @parameters
    if ($calls.Count -ne 5 -or $calls[2][2] -notlike '*Templates*.nupkg') { throw 'Content-only packages must publish without symbols.' }

    Write-Fixture
    $env:NUGET_API_KEY = ''
    Assert-Rejected { & $publishScript @parameters } 'missing credentials'
    $env:NUGET_API_KEY = 'fixture-key'

    Write-Fixture
    Add-Content -LiteralPath (Join-Path $directory 'ModularPipelines.4.2.3.snupkg') -Value 'corrupt'
    Assert-Rejected { & $publishScript @parameters } 'corrupt symbols'
    Write-Fixture
    Remove-Item -LiteralPath (Join-Path $directory 'ModularPipelines.4.2.3.nupkg')
    Assert-Rejected { & $publishScript @parameters } 'missing primary'
    Write-Fixture
    $parameters.ExpectedVersion = '4.2.4'
    Assert-Rejected { & $publishScript @parameters } 'wrong version'
    $parameters.ExpectedVersion = '4.2.3'

    foreach ($mutation in @(
            { param($m) $m.RetrySucceeded = $false },
            { param($m) $m.PackageCount = 0; $m.Files = @() },
            { param($m) $m.PackageCount = 2 },
            { param($m) $m.Files += $m.Files[0] },
            { param($m) $m.Files[0].File = '../outside.nupkg' }
        )) {
        Write-Fixture
        $manifestPath = Join-Path $directory 'manifest.json'
        $manifest = Get-Content -LiteralPath $manifestPath -Raw | ConvertFrom-Json
        & $mutation $manifest
        $manifest | ConvertTo-Json -Depth 5 | Set-Content -LiteralPath $manifestPath
        Assert-Rejected { & $publishScript @parameters } 'invalid rehearsal manifest'
    }
    Write-Output 'Release publication: verified artifacts, version, retry, prerelease, and failure cases passed.'
}
finally {
    $env:NUGET_API_KEY = $originalNugetKey
    $env:GH_TOKEN = $originalGitHubToken
    Remove-Item Function:dotnet, Function:gh
    $resolvedDirectory = [IO.Path]::GetFullPath($directory)
    $fixtureRoot = [IO.Path]::GetFullPath((Join-Path $repositoryRoot 'artifacts/publish-release-tests')) + [IO.Path]::DirectorySeparatorChar
    if (-not $resolvedDirectory.StartsWith($fixtureRoot, [StringComparison]::OrdinalIgnoreCase)) {
        throw 'Refusing to remove a fixture outside the release test directory.'
    }
    if (Test-Path $resolvedDirectory) { Remove-Item -LiteralPath $resolvedDirectory -Recurse -Force }
    $global:LASTEXITCODE = $originalExitCode
}
