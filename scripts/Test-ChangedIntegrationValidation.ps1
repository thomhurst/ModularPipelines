$ErrorActionPreference = 'Stop'
$repositoryRoot = Split-Path $PSScriptRoot -Parent
$resolver = Join-Path $PSScriptRoot 'Resolve-FullPipelineValidation.ps1'

function Assert-IntegrationRoute {
    param(
        [string]$Name,
        [string[]]$Paths,
        [string[]]$ExpectedPackages,
        [bool]$ExpectedFull = $false,
        [hashtable]$AdditionalArguments = @{},
        [string]$EventName = 'pull_request'
    )

    $output = New-TemporaryFile
    try {
        $full = & $resolver -EventName $EventName -ChangedPath $Paths -GitHubOutput $output @AdditionalArguments 6>$null
        $values = @{}
        foreach ($line in Get-Content -LiteralPath $output) {
            $key, $value = $line -split '=', 2
            $values[$key] = $value
        }
        $expectedRequired = ($ExpectedPackages.Count -gt 0).ToString().ToLowerInvariant()
        if ($values.run_integration_validation -ne $expectedRequired -or $full -ne $ExpectedFull) {
            throw "Route '$Name' did not select its required validation: $($values | ConvertTo-Json -Compress)."
        }
        $matrix = $values.integration_matrix | ConvertFrom-Json
        $packages = @($matrix.include | ForEach-Object { $_.package })
        if (($packages -join ',') -ne (($ExpectedPackages | Sort-Object) -join ',')) {
            throw "Route '$Name' selected unexpected packages: $packages."
        }
        foreach ($entry in $matrix.include) {
            $expectedProject = "src/$($entry.package)/$($entry.package).csproj"
            $expectedTest = "test/$($entry.package).UnitTests/$($entry.package).UnitTests.csproj"
            if (-not (Test-Path -LiteralPath (Join-Path $repositoryRoot $expectedTest))) { $expectedTest = '' }
            if ($entry.project -ne $expectedProject -or $entry.test_project -ne $expectedTest) {
                throw "Route '$Name' selected the wrong library or test project."
            }
        }
    }
    finally {
        Remove-Item -LiteralPath $output -Force
    }
}

$google = 'src/ModularPipelines.Google/Options/GcloudExample.Generated.cs'
$generator = 'tools/ModularPipelines.OptionsGenerator/src/Scraper.cs'
Assert-IntegrationRoute 'manual generated options' @($google) @('ModularPipelines.Google')
Assert-IntegrationRoute 'mixed generator and integration' @($generator, $google) @('ModularPipelines.Google')
Assert-IntegrationRoute 'test-only change' @('test/ModularPipelines.Google.UnitTests/ChangedTests.cs') @('ModularPipelines.Google')
Assert-IntegrationRoute 'handwritten integration' @('src/ModularPipelines.Ftp/Ftp.cs') @('ModularPipelines.Ftp')
Assert-IntegrationRoute 'package absent from full test registration' @('src/ModularPipelines.Testing/ModuleTester.cs') @('ModularPipelines.Testing')
Assert-IntegrationRoute 'multiple packages' @($google, 'src/ModularPipelines.DotNet/Options/Changed.Generated.cs') @('ModularPipelines.Google', 'ModularPipelines.DotNet')
Assert-IntegrationRoute 'source and test deduplication' @($google, 'test/ModularPipelines.Google.UnitTests/ChangedTests.cs') @('ModularPipelines.Google')
Assert-IntegrationRoute 'core and integration' @($google, 'src/ModularPipelines/Context/Http.cs') @('ModularPipelines.Google') -ExpectedFull $true
Assert-IntegrationRoute 'core-only' @('src/ModularPipelines/Context/Http.cs') @() -ExpectedFull $true
Assert-IntegrationRoute 'docs-only' @('docs/docs/mp-packages/cli/gcloud.md') @()
Assert-IntegrationRoute 'generator-only' @($generator) @()
Assert-IntegrationRoute 'main push' @($google) @('ModularPipelines.Google') -EventName push
Assert-IntegrationRoute 'library without tests' @('src/ModularPipelines.Slack/Slack.cs') @('ModularPipelines.Slack')
Assert-IntegrationRoute 'Windows separators' @('test\ModularPipelines.Google.UnitTests\ChangedTests.cs') @('ModularPipelines.Google')

# Compose the real safeguarded classifier with the matrix resolver, as the workflow does.
. (Join-Path $PSScriptRoot 'Resolve-GeneratedIntegrationValidation.ps1') -HeadRef ignored -ChangedPath @()
foreach ($case in @(
    @{ Name = 'qualified automation'; HeadRef = 'automated/update-cli-options-gcloud'; Author = 'thomhurst'; HeadRepository = 'thomhurst/ModularPipelines'; Paths = @($google); Generated = $true },
    @{ Name = 'mixed automation'; HeadRef = 'automated/update-cli-options-gcloud'; Author = 'thomhurst'; HeadRepository = 'thomhurst/ModularPipelines'; Paths = @($google, $generator); Generated = $false },
    @{ Name = 'fork using automation branch'; HeadRef = 'automated/update-cli-options-gcloud'; Author = 'contributor'; HeadRepository = 'contributor/ModularPipelines'; Paths = @($google); Generated = $false },
    @{ Name = 'untrusted author'; HeadRef = 'automated/update-cli-options-gcloud'; Author = 'contributor'; HeadRepository = 'thomhurst/ModularPipelines'; Paths = @($google); Generated = $false },
    @{ Name = 'wrong tool branch'; HeadRef = 'automated/update-cli-options-aws'; Author = 'thomhurst'; HeadRepository = 'thomhurst/ModularPipelines'; Paths = @($google); Generated = $false }
)) {
    $classified = Resolve-GeneratedIntegrationValidation -HeadRef $case.HeadRef `
        -HeadRepository $case.HeadRepository -PullRequestAuthor $case.Author `
        -Repository 'thomhurst/ModularPipelines' -RepositoryRoot $repositoryRoot -ChangedPath $case.Paths
    if ($classified.IsGeneratedIntegration -ne $case.Generated) { throw "Unexpected classification: $($case.Name)." }
    $expectedPackages = if ($case.Generated) { @() } else { @('ModularPipelines.Google') }
    Assert-IntegrationRoute $case.Name $case.Paths $expectedPackages -AdditionalArguments @{
        IsGeneratedIntegration = $classified.IsGeneratedIntegration.ToString().ToLowerInvariant()
    }
}

$workflow = Get-Content -LiteralPath (Join-Path $repositoryRoot '.github/workflows/dotnet.yml') -Raw
$matrixJob = [regex]::Match($workflow, '(?ms)^  changed-integration:.*?(?=^  [a-z0-9-]+:|\z)').Value
foreach ($required in @(
    "needs.fast-fail.outputs.run_integration_validation == 'true'",
    'fromJSON(needs.fast-fail.outputs.integration_matrix)',
    'INTEGRATION_PROJECT: ${{ matrix.project }}',
    'TEST_PROJECT: ${{ matrix.test_project }}',
    'dotnet build "$INTEGRATION_PROJECT"',
    'dotnet build "$TEST_PROJECT"',
    'dotnet run --project "$TEST_PROJECT"',
    'contents: read',
    'fail-fast: false'
)) {
    if (-not $matrixJob.Contains($required, [StringComparison]::Ordinal)) {
        throw "Changed integration job omitted '$required'."
    }
}
if (-not $workflow.Contains('-IsGeneratedIntegration $env:IS_GENERATED_INTEGRATION', [StringComparison]::Ordinal) -or
    -not $workflow.Contains('IS_GENERATED_INTEGRATION: ${{ steps.generated_integration.outputs.is_generated_integration }}', [StringComparison]::Ordinal)) {
    throw 'Matrix classification must receive the existing safeguarded generated-PR decision.'
}

$temporaryRoot = [IO.Path]::GetFullPath([IO.Path]::GetTempPath())
$fixture = Join-Path $temporaryRoot "integration-routing-$([guid]::NewGuid().ToString('N'))"
$package = 'ModularPipelines.Example'
$changedResolver = Join-Path $PSScriptRoot 'Resolve-ChangedIntegrationValidation.ps1'
try {
    New-Item -ItemType Directory -Path $fixture | Out-Null
    $removed = & $changedResolver -RepositoryRoot $fixture -ChangedPath @("src/$package/Deleted.cs")
    if ($removed.Required) { throw 'Completely removed packages must not select nonexistent projects.' }

    foreach ($case in @(
        @{ Directory = "src/$package"; Path = "src/$package/Changed.cs"; Project = '' },
        @{ Directory = 'test/ModularPipelines.Orphan.UnitTests'; Path = 'test/ModularPipelines.Orphan.UnitTests/ChangedTests.cs'; Project = 'ModularPipelines.Orphan.UnitTests.csproj' }
    )) {
        $directory = Join-Path $fixture $case.Directory
        New-Item -ItemType Directory -Path $directory -Force | Out-Null
        if ($case.Project) { Set-Content -LiteralPath (Join-Path $directory $case.Project) -Value '<Project />' }
        $failed = $false
        try {
            & $changedResolver -RepositoryRoot $fixture -ChangedPath @($case.Path) | Out-Null
        }
        catch { $failed = $true }
        if (-not $failed) { throw 'An incomplete package layout must not silently skip validation.' }
    }
}
finally {
    $resolvedFixture = [IO.Path]::GetFullPath($fixture)
    if (-not $resolvedFixture.StartsWith($temporaryRoot, [StringComparison]::OrdinalIgnoreCase) -or
        (Split-Path $resolvedFixture -Leaf) -notlike 'integration-routing-*') {
        throw 'Refusing to remove a routing fixture outside the temporary directory.'
    }
    Remove-Item -LiteralPath $resolvedFixture -Recurse -Force
}

Write-Host 'Changed integration validation tests passed.'
