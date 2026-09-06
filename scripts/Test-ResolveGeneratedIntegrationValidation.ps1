$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot 'Resolve-GeneratedIntegrationValidation.ps1') `
    -HeadRef ignored `
    -ChangedPath @()
. (Join-Path $PSScriptRoot 'GeneratedOptionsProvenance.ps1')

function Assert-Equal {
    param(
        [Parameter(Mandatory)]$Actual,
        [Parameter(Mandatory)]$Expected,
        [Parameter(Mandatory)][string]$Message
    )

    if ($Actual -ne $Expected) {
        throw "$Message Expected '$Expected', received '$Actual'."
    }
}

$repositoryRoot = Split-Path $PSScriptRoot -Parent
$repository = 'thomhurst/ModularPipelines'
$automationAuthor = 'thomhurst'
$aws = Resolve-GeneratedIntegrationValidation `
    -HeadRef 'automated/update-cli-options-aws' `
    -HeadRepository $repository `
    -PullRequestAuthor $automationAuthor `
    -Repository $repository `
    -ChangedPath @(
        'docs/docs/mp-packages/cli/aws.md',
        'src/ModularPipelines.AmazonWebServices/AssemblyInfo.Generated.cs',
        'src/ModularPipelines.AmazonWebServices/Enums/AwsMode.Generated.cs',
        'src/ModularPipelines.AmazonWebServices/Generated/Aws.Generation.json',
        'src/ModularPipelines.AmazonWebServices/Options/AwsRunOptions.Generated.cs',
        'src/ModularPipelines.AmazonWebServices/PublicAPI.Unshipped.txt'
    ) `
    -RepositoryRoot $repositoryRoot

Assert-Equal $aws.IsGeneratedIntegration $true 'AWS generated changes should use sharded validation.'
Assert-Equal $aws.Tool 'aws' 'AWS validation selected the wrong tool.'
Assert-Equal $aws.NamespacePrefix 'Aws' 'AWS validation selected the wrong namespace prefix.'
Assert-Equal `
    $aws.Solution `
    'src/ModularPipelines.AmazonWebServices/ModularPipelines.AmazonWebServices.slnx' `
    'AWS validation selected the wrong solution.'
Assert-Equal `
    $aws.TestProject `
    'test/ModularPipelines.AmazonWebServices.UnitTests/ModularPipelines.AmazonWebServices.UnitTests.csproj' `
    'AWS validation selected the wrong test project.'
if ($aws.Solution -match 'Azure|Google') {
    throw "AWS validation selected an unrelated integration: $($aws.Solution)."
}

foreach ($case in @(
        @{ Tool = 'dotnet'; Package = 'ModularPipelines.DotNet'; Prefix = 'DotNet' },
        @{ Tool = 'mvn'; Package = 'ModularPipelines.Java'; Prefix = 'Maven' },
        @{ Tool = 'gradle'; Package = 'ModularPipelines.Java'; Prefix = 'Gradle' },
        @{ Tool = 'kubectl'; Package = 'ModularPipelines.Kubernetes'; Prefix = 'Kubernetes' },
        @{ Tool = 'kustomize'; Package = 'ModularPipelines.Kubernetes'; Prefix = 'Kustomize' }
    )) {
    $parameters = @{
        HeadRef = "automated/update-cli-options-$($case.Tool)"
        HeadRepository = $repository
        PullRequestAuthor = $automationAuthor
        Repository = $repository
        RepositoryRoot = $repositoryRoot
    }
    $shippedPath = "src/$($case.Package)/PublicAPI.Shipped.txt"
    foreach ($paths in @(
            @{ Values = @($shippedPath) },
            @{ Values = @(
                    $shippedPath,
                    "src/$($case.Package)/PublicAPI.Unshipped.txt",
                    "src/$($case.Package)/Generated/$($case.Prefix).Generation.json"
                ) }
        )) {
        $result = Resolve-GeneratedIntegrationValidation @parameters -ChangedPath $paths.Values
        Assert-Equal $result.IsGeneratedIntegration $true "$($case.Tool) baseline updates should use generated validation."
        Assert-Equal $result.NamespacePrefix $case.Prefix 'Shared packages must select the branch tool manifest.'
        Assert-Equal $result.Solution "src/$($case.Package)/$($case.Package).slnx" 'Baseline updates selected the wrong solution.'
    }

    foreach ($unexpectedPath in @(
            '.github/workflows/dotnet.yml',
            'scripts/Resolve-GeneratedIntegrationValidation.ps1',
            'src/ModularPipelines/Engine/ModuleScheduler.cs',
            'src/ModularPipelines.Pulumi/PublicAPI.Shipped.txt',
            "src/$($case.Package)/Handwritten.cs",
            "src/$($case.Package)/Generated/Other.Generation.json"
        )) {
        $result = Resolve-GeneratedIntegrationValidation @parameters `
            -ChangedPath @($shippedPath, $unexpectedPath)
        Assert-Equal $result.IsGeneratedIntegration $false "A shipped baseline must not hide unrelated changes: $unexpectedPath."
    }
}

$unrelatedManifest = Resolve-GeneratedIntegrationValidation `
    -HeadRef 'automated/update-cli-options-aws' `
    -HeadRepository $repository `
    -PullRequestAuthor $automationAuthor `
    -Repository $repository `
    -ChangedPath @(
        'src/ModularPipelines.AmazonWebServices/Generated/Other.Generation.json',
        'src/ModularPipelines.AmazonWebServices/Options/AwsRunOptions.Generated.cs'
    ) `
    -RepositoryRoot $repositoryRoot
Assert-Equal `
    $unrelatedManifest.IsGeneratedIntegration `
    $false `
    'Unrelated generation manifests must retain full validation.'

$missingManifestRoot = Join-Path ([IO.Path]::GetTempPath()) "missing-generated-manifest-$([Guid]::NewGuid().ToString('N'))"
try {
    New-Item -ItemType Directory -Path $missingManifestRoot | Out-Null
    $missingManifest = Resolve-GeneratedIntegrationValidation `
        -HeadRef 'automated/update-cli-options-missing' `
        -HeadRepository $repository `
        -PullRequestAuthor $automationAuthor `
        -Repository $repository `
        -ChangedPath @('src/ModularPipelines.Missing/Options/MissingRunOptions.Generated.cs') `
        -RepositoryRoot $missingManifestRoot
    Assert-Equal `
        $missingManifest.IsGeneratedIntegration `
        $false `
        'Missing manifest directories must retain full validation.'
}
finally {
    Remove-Item -LiteralPath $missingManifestRoot -Recurse -Force -ErrorAction SilentlyContinue
}

$nonPullRequest = Resolve-GeneratedIntegrationValidation `
    -HeadRef 'not-a-generated-pull-request' `
    -HeadRepository '' `
    -PullRequestAuthor '' `
    -Repository $repository `
    -ChangedPath @() `
    -RepositoryRoot $repositoryRoot
Assert-Equal `
    $nonPullRequest.IsGeneratedIntegration `
    $false `
    'Push and workflow-dispatch runs must retain full validation.'

$outputFile = New-TemporaryFile
try {
    & (Join-Path $PSScriptRoot 'Resolve-GeneratedIntegrationValidation.ps1') `
        -HeadRef 'automated/update-cli-options-aws' `
        -HeadRepository $repository `
        -PullRequestAuthor $automationAuthor `
        -Repository $repository `
        -ChangedPath @('src/ModularPipelines.AmazonWebServices/Options/AwsRunOptions.Generated.cs') `
        -RepositoryRoot $repositoryRoot `
        -GitHubOutput $outputFile | Out-Null
    $outputs = @(Get-Content -LiteralPath $outputFile)
    foreach ($expectedOutput in @(
                 'is_generated_integration=true',
                 'tool=aws',
                 'namespace_prefix=Aws',
                 'package=ModularPipelines.AmazonWebServices',
                 'solution=src/ModularPipelines.AmazonWebServices/ModularPipelines.AmazonWebServices.slnx',
                 'test_project=test/ModularPipelines.AmazonWebServices.UnitTests/ModularPipelines.AmazonWebServices.UnitTests.csproj'
             )) {
        if ($outputs -notcontains $expectedOutput) {
            throw "GitHub output omitted '$expectedOutput'."
        }
    }
}
finally {
    Remove-Item -LiteralPath $outputFile -Force
}

$multipleIntegrations = Resolve-GeneratedIntegrationValidation `
    -HeadRef 'automated/update-cli-options-aws' `
    -HeadRepository $repository `
    -PullRequestAuthor $automationAuthor `
    -Repository $repository `
    -ChangedPath @(
        'src/ModularPipelines.AmazonWebServices/Options/AwsRunOptions.Generated.cs',
        'src/ModularPipelines.Google/Options/GcloudRunOptions.Generated.cs'
    ) `
    -RepositoryRoot $repositoryRoot
Assert-Equal `
    $multipleIntegrations.IsGeneratedIntegration `
    $false `
    'Changes spanning integrations must retain full validation.'

$handwrittenChange = Resolve-GeneratedIntegrationValidation `
    -HeadRef 'feature/aws-fix' `
    -HeadRepository $repository `
    -PullRequestAuthor $automationAuthor `
    -Repository $repository `
    -ChangedPath @('src/ModularPipelines.AmazonWebServices/AmazonWebServicesContext.cs') `
    -RepositoryRoot $repositoryRoot
Assert-Equal `
    $handwrittenChange.IsGeneratedIntegration `
    $false `
    'Non-automated branches must retain full validation.'

$automatedHandwrittenChange = Resolve-GeneratedIntegrationValidation `
    -HeadRef 'automated/update-cli-options-aws' `
    -HeadRepository $repository `
    -PullRequestAuthor $automationAuthor `
    -Repository $repository `
    -ChangedPath @('src/ModularPipelines.AmazonWebServices/AmazonWebServicesContext.cs') `
    -RepositoryRoot $repositoryRoot
Assert-Equal `
    $automatedHandwrittenChange.IsGeneratedIntegration `
    $false `
    'Handwritten package files must retain full validation on automated branches.'

$forkedChange = Resolve-GeneratedIntegrationValidation `
    -HeadRef 'automated/update-cli-options-aws' `
    -HeadRepository 'contributor/ModularPipelines' `
    -PullRequestAuthor 'contributor' `
    -Repository $repository `
    -ChangedPath @('src/ModularPipelines.AmazonWebServices/Options/AwsRunOptions.Generated.cs') `
    -RepositoryRoot $repositoryRoot
Assert-Equal `
    $forkedChange.IsGeneratedIntegration `
    $false `
    'Fork pull requests must retain full validation.'

$unownedChange = Resolve-GeneratedIntegrationValidation `
    -HeadRef 'automated/update-cli-options-aws' `
    -HeadRepository $repository `
    -PullRequestAuthor 'contributor' `
    -Repository $repository `
    -ChangedPath @('src/ModularPipelines.AmazonWebServices/Options/AwsRunOptions.Generated.cs') `
    -RepositoryRoot $repositoryRoot
Assert-Equal `
    $unownedChange.IsGeneratedIntegration `
    $false `
    'Non-automation authors must retain full validation.'

$wrongPackage = Resolve-GeneratedIntegrationValidation `
    -HeadRef 'automated/update-cli-options-aws' `
    -HeadRepository $repository `
    -PullRequestAuthor $automationAuthor `
    -Repository $repository `
    -ChangedPath @('src/ModularPipelines.Google/Options/GcloudRunOptions.Generated.cs') `
    -RepositoryRoot $repositoryRoot
Assert-Equal `
    $wrongPackage.IsGeneratedIntegration `
    $false `
    'Generated paths must match the branch tool package.'

$escapedChange = Resolve-GeneratedIntegrationValidation `
    -HeadRef 'automated/update-cli-options-aws' `
    -HeadRepository $repository `
    -PullRequestAuthor $automationAuthor `
    -Repository $repository `
    -ChangedPath @(
        'src/ModularPipelines.AmazonWebServices/Options/AwsRunOptions.Generated.cs',
        'scripts/unrelated.ps1'
    ) `
    -RepositoryRoot $repositoryRoot
Assert-Equal `
    $escapedChange.IsGeneratedIntegration `
    $false `
    'Changes outside the generated package and its documentation must retain full validation.'

$workflow = Get-Content -LiteralPath (Join-Path $repositoryRoot '.github/workflows/dotnet.yml') -Raw
$fastFailJob = [regex]::Match(
    $workflow,
    '(?ms)^  fast-fail:.*?(?=^  analyzers:)').Value
foreach ($freshnessGuard in @(
             'Reject stale generated snapshots',
             'Assert-GeneratedOptionsFreshness.ps1',
             'integration_namespace_prefix'
         )) {
    if (-not $fastFailJob.Contains($freshnessGuard, [StringComparison]::Ordinal)) {
        throw "Fast-fail validation omitted generated freshness guard '$freshnessGuard'."
    }
}

$generatedJob = [regex]::Match(
    $workflow,
    '(?ms)^  generated-integration:.*?(?=^  trim-aot:)').Value
if ([string]::IsNullOrWhiteSpace($generatedJob)) {
    throw 'Generated integration validation job was not found.'
}

if ($generatedJob.Contains(
        'tools/ModularPipelines.OptionsGenerator/ModularPipelines.OptionsGenerator.slnx',
        [StringComparison]::Ordinal)) {
    throw 'Generated integration validation must not build the unrelated OptionsGenerator solution.'
}

foreach ($gcloudSafeguard in @(
             'Increase swap space for generated gcloud validation',
             'sudo fallocate -l 10G /mnt/swapfile',
             '/m:1 -p:UseSharedCompilation=false'
         )) {
    if (-not $generatedJob.Contains($gcloudSafeguard, [StringComparison]::Ordinal)) {
        throw "Generated gcloud validation omitted safeguard '$gcloudSafeguard'."
    }
}

$generationWorkflow = Get-Content `
    -LiteralPath (Join-Path $repositoryRoot '.github/workflows/generate-cli-options.yml') `
    -Raw
foreach ($refreshBehavior in @(
             'push:',
             'tools/ModularPipelines.OptionsGenerator/**',
             'Write-GeneratedOptionsProvenance.ps1',
             "github.event_name == 'schedule' || (github.event_name == 'workflow_dispatch'"
         )) {
    if (-not $generationWorkflow.Contains($refreshBehavior, [StringComparison]::Ordinal)) {
        throw "Generated-options refresh workflow omitted '$refreshBehavior'."
    }
}
foreach ($sourcePath in Get-GeneratedOptionsSourcePath) {
    if (-not $generationWorkflow.Contains($sourcePath, [StringComparison]::Ordinal)) {
        throw "Generated-options refresh trigger omitted source input '$sourcePath'."
    }
}

Write-Host 'Generated integration validation resolver tests passed.'
