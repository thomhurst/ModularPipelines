$ErrorActionPreference = 'Stop'

$syncScript = Join-Path $PSScriptRoot 'Sync-PublicApiBaselines.ps1'
$testRoot = Join-Path ([IO.Path]::GetTempPath()) (
    "sync-public-api-baselines-{0}" -f [guid]::NewGuid())
$resolvedTempRoot = [IO.Path]::GetFullPath([IO.Path]::GetTempPath())
$resolvedTestRoot = [IO.Path]::GetFullPath($testRoot)
if (-not $resolvedTestRoot.StartsWith(
        $resolvedTempRoot,
        [StringComparison]::OrdinalIgnoreCase)) {
    throw "Refusing to use test directory outside temp root: $resolvedTestRoot"
}

New-Item -ItemType Directory -Path $resolvedTestRoot | Out-Null

try {
    $packageDirectory = Join-Path $resolvedTestRoot 'src/Target.Package'
    $temporaryDirectory = Join-Path $resolvedTestRoot 'temp'
    New-Item -ItemType Directory -Path $packageDirectory, $temporaryDirectory | Out-Null
    $projectPath = Join-Path $packageDirectory 'Target.Package.csproj'
    Set-Content -LiteralPath $projectPath -Value '<Project />'
    @('#nullable enable', 'Api.Existing', 'Api.Removed') |
        Set-Content -LiteralPath (Join-Path $packageDirectory 'PublicAPI.Shipped.txt')
    @('#nullable enable', 'Api.Draft', 'Api.RemovedDraft') |
        Set-Content -LiteralPath (Join-Path $packageDirectory 'PublicAPI.Unshipped.txt')

    $fakeDotNet = Join-Path $resolvedTestRoot 'fake-dotnet.ps1'
    $fakeDotNetSource = @'
param([Parameter(ValueFromRemainingArguments)][string[]] $RemainingArguments)
$errorLogArgument = $RemainingArguments | Where-Object { $_ -like '-p:ErrorLog=*' }
if (-not $errorLogArgument) { throw 'Missing ErrorLog argument.' }
if ($RemainingArguments -notcontains '-test-extra') { throw 'Missing extra build argument.' }
$errorLogPath = $errorLogArgument.Substring('-p:ErrorLog='.Length)
$count = if (Test-Path -LiteralPath $env:SYNC_TEST_COUNT_FILE) {
    [int] (Get-Content -LiteralPath $env:SYNC_TEST_COUNT_FILE -Raw)
} else { 0 }
$count++
Set-Content -LiteralPath $env:SYNC_TEST_COUNT_FILE -Value $count
if ($env:SYNC_TEST_FAIL_BUILD -and $count -eq [int] $env:SYNC_TEST_FAIL_BUILD) {
    $global:LASTEXITCODE = 1
    return
}
$targetUri = [Uri]::new((Join-Path $env:SYNC_TEST_PACKAGE 'Generated.cs')).AbsoluteUri
$isRemovalBuild = $errorLogPath.EndsWith('PublicAPI.removals.sarif', [StringComparison]::Ordinal)
$shipped = @(Get-Content -LiteralPath (Join-Path $env:SYNC_TEST_PACKAGE 'PublicAPI.Shipped.txt'))
$unshipped = @(Get-Content -LiteralPath (Join-Path $env:SYNC_TEST_PACKAGE 'PublicAPI.Unshipped.txt'))
if ($count -eq 1) {
    if ($isRemovalBuild -or $shipped.Count -ne 1 -or $unshipped.Count -ne 1) {
        throw 'Discover the current API from empty baselines before collecting removals.'
    }
} elseif (-not $isRemovalBuild -or
    $shipped -notcontains 'Api.Removed' -or
    $unshipped -notcontains 'Api.RemovedDraft' -or
    $unshipped -notcontains 'Api.Added' -or
    @($unshipped | Where-Object { $_ -eq 'Api.Draft' }).Count -ne 1 -or
    $unshipped -contains 'Api.Existing') {
    throw 'Removal discovery must retain old APIs and declare current additions without duplicates.'
}
$messageSuffix = if ($isRemovalBuild) {
    ' is part of the declared API, but is either not public or could not be found'
} else {
    ' is not part of the declared public API'
}
$symbols = if ($isRemovalBuild) { @('Api.Removed', 'Api.RemovedDraft') } else { @('Api.Existing', 'Api.Draft', 'Api.Added') }
$ruleId = if ($isRemovalBuild) { 'RS0017' } else { 'RS0016' }
$results = @($symbols | ForEach-Object {
    @{
        ruleId = $ruleId
        message = "Symbol '$_'$messageSuffix"
        locations = @(@{
            physicalLocation = @{ artifactLocation = @{ uri = $targetUri } }
        })
    }
})
@{ version = '2.1.0'; runs = @(@{ results = $results }) } |
    ConvertTo-Json -Depth 10 |
    Set-Content -LiteralPath $errorLogPath
$global:LASTEXITCODE = 0
'@
    [IO.File]::WriteAllText($fakeDotNet, $fakeDotNetSource, [Text.UTF8Encoding]::new($false))

    $countFile = Join-Path $resolvedTestRoot 'build-count.txt'
    $env:SYNC_TEST_COUNT_FILE = $countFile
    $env:SYNC_TEST_PACKAGE = $packageDirectory
    & $syncScript `
        -PackageDirectory $packageDirectory `
        -ProjectPath $projectPath `
        -TemporaryDirectory $temporaryDirectory `
        -ExtraBuildArguments @('-test-extra') `
        -DotNetExecutable $fakeDotNet

    $actualShipped = @(Get-Content -LiteralPath (Join-Path $packageDirectory 'PublicAPI.Shipped.txt'))
    $actualUnshipped = @(Get-Content -LiteralPath (Join-Path $packageDirectory 'PublicAPI.Unshipped.txt'))
    $expectedShipped = @('#nullable enable', 'Api.Existing', 'Api.Removed')
    $expectedUnshipped = @('#nullable enable', '*REMOVED*Api.Removed', 'Api.Added', 'Api.Draft')
    if (-not [Linq.Enumerable]::SequenceEqual(
            [string[]] $actualShipped,
            [string[]] $expectedShipped,
            [StringComparer]::Ordinal) -or
        -not [Linq.Enumerable]::SequenceEqual(
            [string[]] $actualUnshipped,
            [string[]] $expectedUnshipped,
            [StringComparer]::Ordinal)) {
        throw 'Unexpected synchronized public API baselines.'
    }

    if ((Get-Content -LiteralPath $countFile -Raw).Trim() -ne '2') {
        throw 'Expected exactly two public API builds.'
    }

    foreach ($failedBuild in 1, 2) {
        $initialShipped = @('#nullable enable', 'Api.Existing', 'Api.Removed')
        $initialUnshipped = @('#nullable enable', 'Api.Draft', 'Api.RemovedDraft')
        $initialShipped | Set-Content -LiteralPath (Join-Path $packageDirectory 'PublicAPI.Shipped.txt')
        $initialUnshipped | Set-Content -LiteralPath (Join-Path $packageDirectory 'PublicAPI.Unshipped.txt')
        Set-Content -LiteralPath $countFile -Value 0
        $env:SYNC_TEST_FAIL_BUILD = [string] $failedBuild
        $priorExitCode = $global:LASTEXITCODE
        $failure = $null
        try {
            & $syncScript `
                -PackageDirectory $packageDirectory `
                -ProjectPath $projectPath `
                -TemporaryDirectory $temporaryDirectory `
                -ExtraBuildArguments @('-test-extra') `
                -DotNetExecutable $fakeDotNet
        }
        catch {
            $failure = $_
        }
        finally {
            $global:LASTEXITCODE = $priorExitCode
        }

        if ($null -eq $failure -or
            $failure.Exception.Message -notlike 'Failed to * public API *') {
            throw "Expected a public API build failure in pass $failedBuild."
        }
        foreach ($baseline in 'PublicAPI.Shipped.txt', 'PublicAPI.Unshipped.txt') {
            $actual = [IO.File]::ReadAllBytes((Join-Path $packageDirectory $baseline))
            $original = [IO.File]::ReadAllBytes((Join-Path $temporaryDirectory ($baseline.Replace('.txt', '.original.txt'))))
            if (-not [Linq.Enumerable]::SequenceEqual([byte[]] $actual, [byte[]] $original)) {
                throw "Failed pass $failedBuild did not restore $baseline exactly."
            }
        }
    }

    Write-Output 'OK public API synchronization orchestration passed.'
}
finally {
    Remove-Item Env:SYNC_TEST_COUNT_FILE -ErrorAction SilentlyContinue
    Remove-Item Env:SYNC_TEST_PACKAGE -ErrorAction SilentlyContinue
    Remove-Item Env:SYNC_TEST_FAIL_BUILD -ErrorAction SilentlyContinue
    if (Test-Path -LiteralPath $resolvedTestRoot) {
        Remove-Item -LiteralPath $resolvedTestRoot -Recurse -Force
    }
}
