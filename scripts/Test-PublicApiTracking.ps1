$ErrorActionPreference = 'Stop'
$repositoryRoot = Split-Path $PSScriptRoot -Parent
$testRoot = Join-Path ([System.IO.Path]::GetTempPath()) ("public-api-tracking-{0}" -f [guid]::NewGuid())
$props = [System.Security.SecurityElement]::Escape((Join-Path $repositoryRoot 'Directory.Build.props'))

try {
    $fixture = @'
<Project>
  <Import Project="{0}" />
  <Target Name="Check">
    <ItemGroup>
      <ApiAnalyzer Include="@(PackageReference)" Condition="'%(PackageReference.Identity)' == 'Microsoft.CodeAnalysis.PublicApiAnalyzers'" />
    </ItemGroup>
    <Error Condition="'@(ApiAnalyzer)' != '$(ExpectedAnalyzer)'" Text="Unexpected API analyzer selection for $(MSBuildProjectDirectory)." />
    <Error Condition="'$(ExpectedAnalyzer)' != '' and !$([System.String]::Copy('$(WarningsAsErrors)').Contains('RS0016;RS0017'))" Text="API declaration diagnostics must be errors." />
    <Error Condition="'$(ExpectedAnalyzer)' != '' and '$(GITHUB_ACTIONS)' == 'true' and ('$(RunAnalyzers)' != 'true' or '$(RunAnalyzersDuringBuild)' != 'true')" Text="API tracking must run in ordinary CI builds." />
  </Target>
</Project>
'@ -f $props

    foreach ($name in 'handwritten', 'generated', 'untracked') {
        $directory = Join-Path $testRoot $name
        New-Item -ItemType Directory -Path $directory -Force | Out-Null
        Set-Content -LiteralPath (Join-Path $directory 'Fixture.proj') -Value $fixture
        if ($name -ne 'untracked') {
            foreach ($baseline in 'PublicAPI.Shipped.txt', 'PublicAPI.Unshipped.txt') {
                Set-Content -LiteralPath (Join-Path $directory $baseline) -Value '#nullable enable'
            }
        }
    }

    # Even accidental baseline files must not opt generated CLI packages into tracking.
    $generatedDirectory = Join-Path $testRoot 'generated/Generated'
    New-Item -ItemType Directory -Path $generatedDirectory | Out-Null
    Set-Content -LiteralPath (Join-Path $generatedDirectory 'fixture.CommandCoverage.json') -Value '{}'

    $checks = foreach ($name in 'handwritten', 'generated', 'untracked') {
        foreach ($mode in 'local', 'ci', 'analyzers', 'ci-analyzers') {
            $ci = if ($mode -in 'ci', 'ci-analyzers') { 'true' } else { 'false' }
            $analyzers = if ($mode -in 'analyzers', 'ci-analyzers') { 'true' } else { 'false' }
            $expected = if ($name -eq 'handwritten' -and $mode -ne 'local') { 'Microsoft.CodeAnalysis.PublicApiAnalyzers' } else { '' }
            '<MSBuild Projects="{0}/Fixture.proj" Targets="Check" Properties="GITHUB_ACTIONS={1};EnableCiAnalyzers={2};ExpectedAnalyzer={3}" />' -f $name, $ci, $analyzers, $expected
        }
    }
    $project = Join-Path $testRoot 'Tests.proj'
    Set-Content -LiteralPath $project -Value ('<Project><Target Name="Test">' + ($checks -join "`n") + '</Target></Project>')
    # A child PowerShell preserves cleanup because the guard exits its host process.
    $guard = Join-Path $PSScriptRoot 'Invoke-AgentDotNet.ps1'
    $command = "& '{0}' -SingleNode -TimeoutSeconds 60 -DotNetArguments @('msbuild', '{1}', '-target:Test', '-verbosity:minimal')" -f $guard.Replace("'", "''"), $project.Replace("'", "''")
    & pwsh -NoProfile -Command $command
    if ($LASTEXITCODE -ne 0) { throw "Public API tracking checks failed with exit code $LASTEXITCODE." }
    Write-Output "Public API tracking checks passed ($($checks.Count) configurations)."

    $compilationDirectory = Join-Path $testRoot 'compiler'
    New-Item -ItemType Directory -Path $compilationDirectory | Out-Null
    $packages = [System.Security.SecurityElement]::Escape((Join-Path $repositoryRoot 'Directory.Packages.props'))
    $compilationProject = Join-Path $compilationDirectory 'ApiTracking.csproj'
    Set-Content -LiteralPath $compilationProject -Value (@'
<Project>
  <PropertyGroup>
    <DirectoryBuildPropsPath>{0}</DirectoryBuildPropsPath>
    <DirectoryPackagesPropsPath>{1}</DirectoryPackagesPropsPath>
  </PropertyGroup>
  <Import Project="Sdk.props" Sdk="Microsoft.NET.Sdk" />
  <ItemGroup>
    <!-- Exercise API tracking without building the repository's unrelated analyzers. -->
    <ProjectReference Remove="@(ProjectReference)" />
    <PackageReference Remove="Microsoft.SourceLink.GitHub;StyleCop.Analyzers" />
  </ItemGroup>
  <Import Project="Sdk.targets" Sdk="Microsoft.NET.Sdk" />
</Project>
'@ -f $props, $packages)
    Set-Content -LiteralPath (Join-Path $compilationDirectory 'Example.cs') -Value 'namespace ApiTracking; public static class Example { public static void Run() { } }'
    $shipped = Join-Path $compilationDirectory 'PublicAPI.Shipped.txt'
    $unshipped = Join-Path $compilationDirectory 'PublicAPI.Unshipped.txt'
    Set-Content -LiteralPath $shipped -Value '#nullable enable'
    Set-Content -LiteralPath $unshipped -Value '#nullable enable'

    function Assert-CompilerResult([string] $ExpectedDiagnostic = '') {
        $command = "& '{0}' -SingleNode -TimeoutSeconds 120 -DotNetArguments @('build', '{1}', '--no-incremental', '-p:GITHUB_ACTIONS=true', '-p:EnableCiAnalyzers=false')" -f $guard.Replace("'", "''"), $compilationProject.Replace("'", "''")
        $output = @(& pwsh -NoProfile -Command $command 2>&1)
        $exitCode = $LASTEXITCODE
        if ($exitCode -in 124, 137) {
            throw "Compiler fixture reached its validation limit (exit $exitCode): $($output -join "`n")"
        }
        if ($ExpectedDiagnostic) {
            if ($exitCode -eq 0 -or ($output -join "`n") -notmatch "error ${ExpectedDiagnostic}:") {
                throw "Expected compiler error $ExpectedDiagnostic, got exit ${exitCode}: $($output -join "`n")"
            }
        }
        elseif ($exitCode -ne 0) {
            throw "Expected a successful compiler fixture, got exit ${exitCode}: $($output -join "`n")"
        }
    }

    Assert-CompilerResult 'RS0016'
    $currentApi = "#nullable enable`nApiTracking.Example`nstatic ApiTracking.Example.Run() -> void"
    Set-Content -LiteralPath $unshipped -Value $currentApi
    Assert-CompilerResult
    Add-Content -LiteralPath $unshipped -Value 'ApiTracking.Removed'
    Assert-CompilerResult 'RS0017'

    # The same untracked API must compile when the package is a generated CLI snapshot.
    Set-Content -LiteralPath $unshipped -Value '#nullable enable'
    $coverageDirectory = Join-Path $compilationDirectory 'Generated'
    New-Item -ItemType Directory -Path $coverageDirectory | Out-Null
    Set-Content -LiteralPath (Join-Path $coverageDirectory 'fixture.CommandCoverage.json') -Value '{}'
    Assert-CompilerResult
    Write-Output 'Public API compiler checks passed (missing API, valid API, stale API, generated exclusion).'
}
finally {
    $resolvedRoot = [System.IO.Path]::GetFullPath($testRoot)
    $temporaryRoot = [System.IO.Path]::GetFullPath([System.IO.Path]::GetTempPath())
    if (-not $resolvedRoot.StartsWith($temporaryRoot, [System.StringComparison]::OrdinalIgnoreCase)) {
        throw "Refusing cleanup outside the temporary directory: $resolvedRoot"
    }
    if (Test-Path -LiteralPath $resolvedRoot) { Remove-Item -LiteralPath $resolvedRoot -Recurse -Force }
}
