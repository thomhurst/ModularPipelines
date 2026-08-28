$ErrorActionPreference = 'Stop'

$stagingScript = Join-Path $PSScriptRoot 'Stage-GeneratedChanges.ps1'
$testRoot = Join-Path ([System.IO.Path]::GetTempPath()) (
    "stage-generated-changes-{0}" -f [guid]::NewGuid())
$manifest = "$testRoot.manifest"
$resolvedTempRoot = [System.IO.Path]::GetFullPath(
    [System.IO.Path]::GetTempPath())
$resolvedTestRoot = [System.IO.Path]::GetFullPath($testRoot)
if (-not $resolvedTestRoot.StartsWith(
        $resolvedTempRoot,
        [System.StringComparison]::OrdinalIgnoreCase)) {
    throw "Refusing to use test directory outside temp root: $resolvedTestRoot"
}

New-Item -ItemType Directory -Path $testRoot | Out-Null

try {
    & git -C $testRoot init --initial-branch main --quiet
    & git -C $testRoot config user.email generator-test@example.invalid
    & git -C $testRoot config user.name 'Generator Safety Test'

    $generatedDirectory = Join-Path $testRoot 'src/Fake/Options'
    New-Item -ItemType Directory -Path $generatedDirectory | Out-Null
    $generatedFile = Join-Path $generatedDirectory 'FakeOptions.Generated.cs'
    Set-Content -LiteralPath $generatedFile -Value '// original'
    Set-Content -LiteralPath (Join-Path $testRoot '.gitignore') -Value '/*.zip'
    & git -C $testRoot add .
    & git -C $testRoot commit --quiet -m baseline

    Set-Content -LiteralPath $generatedFile -Value '// regenerated'
    Set-Content -LiteralPath $manifest -Value 'src/Fake/Options/FakeOptions.Generated.cs'

    # Simulate a scraper downloading an ignored archive into checkout.
    [System.IO.File]::WriteAllBytes(
        (Join-Path $testRoot 'download.zip'),
        [byte[]] (1, 2, 3, 4))

    $archiveRejected = $false
    try {
        & $stagingScript -RepositoryRoot $testRoot -ManifestPath $manifest
    }
    catch {
        $archiveRejected = $_.Exception.Message.Contains('download.zip')
    }

    if (-not $archiveRejected) {
        throw 'Checkout archive was not rejected.'
    }

    $stagedPaths = @(& git -C $testRoot diff --cached --name-only)
    if ($stagedPaths.Count -ne 0) {
        throw "Archive rejection staged paths unexpectedly: $($stagedPaths -join ', ')"
    }

    Remove-Item -LiteralPath (Join-Path $testRoot 'download.zip') -Force
    & $stagingScript -RepositoryRoot $testRoot -ManifestPath $manifest
    $stagedPaths = @(& git -C $testRoot diff --cached --name-only)
    if (($stagedPaths.Count -ne 1) -or
        ($stagedPaths[0] -ne 'src/Fake/Options/FakeOptions.Generated.cs')) {
        throw "Exact generated path was not staged: $($stagedPaths -join ', ')"
    }

    & git -C $testRoot commit --quiet -m generated

    $obsoleteFile = Join-Path $testRoot 'src/Fake/Options/Obsolete.Generated.cs'
    Set-Content -LiteralPath $obsoleteFile -Value '// obsolete'
    & git -C $testRoot add $obsoleteFile
    & git -C $testRoot commit --quiet -m obsolete
    & git -C $testRoot rm --quiet $obsoleteFile
    Set-Content -LiteralPath $manifest -Value 'src/Fake/Options/Obsolete.Generated.cs'

    & $stagingScript -RepositoryRoot $testRoot -ManifestPath $manifest
    $stagedPaths = @(& git -C $testRoot diff --cached --name-only)
    if (($stagedPaths.Count -ne 1) -or
        ($stagedPaths[0] -ne 'src/Fake/Options/Obsolete.Generated.cs')) {
        throw "Already-staged deletion was not preserved: $($stagedPaths -join ', ')"
    }

    & git -C $testRoot commit --quiet -m cleanup

    $documentationDirectory = Join-Path $testRoot 'docs'
    New-Item -ItemType Directory -Path $documentationDirectory | Out-Null
    $largeFile = Join-Path $documentationDirectory 'generated.md'
    Set-Content -LiteralPath $largeFile -Value ('x' * 2048)
    Set-Content -LiteralPath $manifest -Value 'docs/generated.md'

    $largeFileRejected = $false
    $largeFileError = ''
    try {
        & $stagingScript `
            -RepositoryRoot $testRoot `
            -ManifestPath $manifest `
            -MaximumFileSizeBytes 1024
    }
    catch {
        $largeFileError = $_.Exception.Message
        $largeFileRejected = $_.Exception.Message.Contains('generated.md')
    }

    if (-not $largeFileRejected) {
        throw "Oversized generated file was not rejected. Error: $largeFileError"
    }

    & $stagingScript `
        -RepositoryRoot $testRoot `
        -ManifestPath $manifest `
        -AllowedOversizedPath 'docs/generated.md' `
        -MaximumFileSizeBytes 1024
    $stagedPaths = @(& git -C $testRoot diff --cached --name-only)
    if (($stagedPaths.Count -ne 1) -or
        ($stagedPaths[0] -ne 'docs/generated.md')) {
        throw "Explicitly allowed oversized path was not staged: $($stagedPaths -join ', ')"
    }

    Write-Output 'OK exact staging, archive rejection, staged deletion, and size controls passed.'
}
finally {
    if (Test-Path -LiteralPath $resolvedTestRoot) {
        Remove-Item -LiteralPath $resolvedTestRoot -Recurse -Force
    }
    Remove-Item -LiteralPath $manifest -Force -ErrorAction SilentlyContinue
}
