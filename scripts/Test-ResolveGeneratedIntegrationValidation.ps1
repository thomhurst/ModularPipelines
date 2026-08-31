$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot 'Resolve-GeneratedIntegrationValidation.ps1') `
    -HeadRef ignored `
    -ChangedPath @()

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
$aws = Resolve-GeneratedIntegrationValidation `
    -HeadRef 'automated/update-cli-options-aws' `
    -ChangedPath @(
        'docs/docs/mp-packages/cli/aws.md',
        'src/ModularPipelines.AmazonWebServices/AssemblyInfo.Generated.cs',
        'src/ModularPipelines.AmazonWebServices/Enums/AwsMode.Generated.cs',
        'src/ModularPipelines.AmazonWebServices/Options/AwsRunOptions.Generated.cs',
        'src/ModularPipelines.AmazonWebServices/PublicAPI.Unshipped.txt'
    ) `
    -RepositoryRoot $repositoryRoot

Assert-Equal $aws.IsGeneratedIntegration $true 'AWS generated changes should use sharded validation.'
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

$outputFile = New-TemporaryFile
try {
    & (Join-Path $PSScriptRoot 'Resolve-GeneratedIntegrationValidation.ps1') `
        -HeadRef 'automated/update-cli-options-aws' `
        -ChangedPath @('src/ModularPipelines.AmazonWebServices/Options/AwsRunOptions.Generated.cs') `
        -RepositoryRoot $repositoryRoot `
        -GitHubOutput $outputFile | Out-Null
    $outputs = @(Get-Content -LiteralPath $outputFile)
    foreach ($expectedOutput in @(
                 'is_generated_integration=true',
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
    -ChangedPath @('src/ModularPipelines.AmazonWebServices/AmazonWebServicesContext.cs') `
    -RepositoryRoot $repositoryRoot
Assert-Equal `
    $handwrittenChange.IsGeneratedIntegration `
    $false `
    'Non-automated branches must retain full validation.'

$escapedChange = Resolve-GeneratedIntegrationValidation `
    -HeadRef 'automated/update-cli-options-aws' `
    -ChangedPath @(
        'src/ModularPipelines.AmazonWebServices/Options/AwsRunOptions.Generated.cs',
        'scripts/unrelated.ps1'
    ) `
    -RepositoryRoot $repositoryRoot
Assert-Equal `
    $escapedChange.IsGeneratedIntegration `
    $false `
    'Changes outside the generated package and its documentation must retain full validation.'

Write-Host 'Generated integration validation resolver tests passed.'
