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
    @('#nullable enable') |
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
$targetUri = [Uri]::new((Join-Path $env:SYNC_TEST_PACKAGE 'Generated.cs')).AbsoluteUri
$messageSuffix = if ($count -eq 1) {
    ' is part of the declared API, but is either not public or could not be found'
} else {
    ' is not part of the declared public API'
}
$symbols = if ($count -eq 1) { @('Api.Removed') } else { @('Api.Existing', 'Api.Added') }
$ruleId = if ($count -eq 1) { 'RS0017' } else { 'RS0016' }
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
    $expectedUnshipped = @('#nullable enable', '*REMOVED*Api.Removed', 'Api.Added')
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

    Write-Output 'OK public API synchronization orchestration passed.'
}
finally {
    Remove-Item Env:SYNC_TEST_COUNT_FILE -ErrorAction SilentlyContinue
    Remove-Item Env:SYNC_TEST_PACKAGE -ErrorAction SilentlyContinue
    if (Test-Path -LiteralPath $resolvedTestRoot) {
        Remove-Item -LiteralPath $resolvedTestRoot -Recurse -Force
    }
}
