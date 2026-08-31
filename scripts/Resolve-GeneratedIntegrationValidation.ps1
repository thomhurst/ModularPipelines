[CmdletBinding()]
param(
    [Parameter(Mandatory)][string]$HeadRef,
    [Parameter(Mandatory)][AllowEmptyCollection()][string[]]$ChangedPath,
    [string]$RepositoryRoot = (Split-Path $PSScriptRoot -Parent),
    [string]$GitHubOutput
)

$ErrorActionPreference = 'Stop'

function New-GeneratedIntegrationValidationResult {
    param(
        [Parameter(Mandatory)][bool]$IsGeneratedIntegration,
        [Parameter(Mandatory)][string]$Reason,
        [string]$Package = '',
        [string]$Solution = '',
        [string]$TestProject = ''
    )

    [pscustomobject]@{
        IsGeneratedIntegration = $IsGeneratedIntegration
        Package = $Package
        Solution = $Solution
        TestProject = $TestProject
        Reason = $Reason
    }
}

function Resolve-GeneratedIntegrationValidation {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)][string]$HeadRef,
        [Parameter(Mandatory)][AllowEmptyCollection()][string[]]$ChangedPath,
        [Parameter(Mandatory)][string]$RepositoryRoot
    )

    $branchPrefix = 'automated/update-cli-options-'
    if (-not $HeadRef.StartsWith($branchPrefix, [StringComparison]::Ordinal)) {
        return New-GeneratedIntegrationValidationResult `
            -IsGeneratedIntegration $false `
            -Reason 'The pull request does not use an automated generated-options branch.'
    }

    $tool = $HeadRef.Substring($branchPrefix.Length)
    if ([string]::IsNullOrWhiteSpace($tool)) {
        return New-GeneratedIntegrationValidationResult `
            -IsGeneratedIntegration $false `
            -Reason 'The generated-options branch does not identify a tool.'
    }

    $paths = @($ChangedPath |
        ForEach-Object { $_.Trim().Replace('\', '/') } |
        Where-Object { $_ } |
        Sort-Object -Unique)
    if ($paths.Count -eq 0) {
        return New-GeneratedIntegrationValidationResult `
            -IsGeneratedIntegration $false `
            -Reason 'No changed paths were supplied.'
    }

    $packages = @($paths |
        ForEach-Object {
            if ($_ -match '^src/(?<package>ModularPipelines\.[^/]+)/') {
                $Matches.package
            }
        } |
        Sort-Object -Unique)
    if ($packages.Count -ne 1) {
        return New-GeneratedIntegrationValidationResult `
            -IsGeneratedIntegration $false `
            -Reason "Expected one changed integration package; found $($packages.Count)."
    }

    $package = $packages[0]
    $packagePrefix = "src/$package/"
    $documentationPath = "docs/docs/mp-packages/cli/$tool.md"
    $unexpectedPaths = @($paths | Where-Object {
        -not $_.StartsWith($packagePrefix, [StringComparison]::OrdinalIgnoreCase) -and
        -not $_.Equals($documentationPath, [StringComparison]::OrdinalIgnoreCase)
    })
    if ($unexpectedPaths.Count -gt 0) {
        return New-GeneratedIntegrationValidationResult `
            -IsGeneratedIntegration $false `
            -Reason "Changes escape the generated integration: $($unexpectedPaths -join ', ')."
    }

    $solution = "src/$package/$package.slnx"
    if (-not (Test-Path -LiteralPath (Join-Path $RepositoryRoot $solution) -PathType Leaf)) {
        return New-GeneratedIntegrationValidationResult `
            -IsGeneratedIntegration $false `
            -Reason "Integration solution does not exist: $solution."
    }

    $testProject = "test/$package.UnitTests/$package.UnitTests.csproj"
    if (-not (Test-Path -LiteralPath (Join-Path $RepositoryRoot $testProject) -PathType Leaf)) {
        $testProject = ''
    }

    return New-GeneratedIntegrationValidationResult `
        -IsGeneratedIntegration $true `
        -Package $package `
        -Solution $solution `
        -TestProject $testProject `
        -Reason "All changes belong to generated tool '$tool' and package '$package'."
}

if ($MyInvocation.InvocationName -ne '.') {
    $result = Resolve-GeneratedIntegrationValidation `
        -HeadRef $HeadRef `
        -ChangedPath $ChangedPath `
        -RepositoryRoot $RepositoryRoot
    Write-Host $result.Reason

    if ($GitHubOutput) {
        "is_generated_integration=$($result.IsGeneratedIntegration.ToString().ToLowerInvariant())" |
            Out-File -FilePath $GitHubOutput -Append
        "package=$($result.Package)" | Out-File -FilePath $GitHubOutput -Append
        "solution=$($result.Solution)" | Out-File -FilePath $GitHubOutput -Append
        "test_project=$($result.TestProject)" | Out-File -FilePath $GitHubOutput -Append
    }

    $result
}
