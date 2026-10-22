param(
    [string]$DotNetPath = 'dotnet',
    [string]$ArtifactsPath = [System.IO.Path]::GetTempPath()
)

$ErrorActionPreference = 'Stop'
$repositoryRoot = Split-Path $PSScriptRoot -Parent
$fixtureRoot = Join-Path ([System.IO.Path]::GetFullPath($ArtifactsPath)) ("fsharp-formatting-{0}" -f [guid]::NewGuid())
$guard = Join-Path $PSScriptRoot 'Invoke-AgentDotNet.ps1'
$pwsh = (Get-Process -Id $PID).Path

function Invoke-FormattingCheckDotNet([string[]]$Arguments, [string]$LogName) {
    # An outer native process captures the guard's inherited console output too.
    $quotedArguments = @($Arguments | ForEach-Object { "'{0}'" -f $_.Replace("'", "''") })
    $command = "& '{0}' -DotNetPath '{1}' -DotNetArguments @({2})" -f `
        $guard.Replace("'", "''"), $DotNetPath.Replace("'", "''"), ($quotedArguments -join ', ')
    $command += '; exit $LASTEXITCODE'
    $output = @(& $pwsh -NoProfile -Command $command 2>&1)
    $exitCode = $LASTEXITCODE
    Set-Content -LiteralPath (Join-Path $fixtureRoot $LogName) -Value ($output -join "`n")
    if ($exitCode -in 124, 137) { throw "Validation limit reached: exit $exitCode. See $fixtureRoot/$LogName" }
    return @{ ExitCode = $exitCode; Output = $output -join "`n" }
}

New-Item -ItemType Directory -Path $fixtureRoot | Out-Null
Copy-Item -LiteralPath (Join-Path $repositoryRoot 'global.json') -Destination $fixtureRoot
New-Item -ItemType Directory -Path (Join-Path $fixtureRoot 'fsharp'), (Join-Path $fixtureRoot 'csharp'), (Join-Path $fixtureRoot 'ordering') | Out-Null
Set-Content -LiteralPath (Join-Path $fixtureRoot '.editorconfig') -Value @'
root = true
[*.cs]
dotnet_diagnostic.IDE0005.severity = warning
dotnet_diagnostic.CA1822.severity = warning
'@
Set-Content -LiteralPath (Join-Path $fixtureRoot 'fsharp/Fixture.fsproj') -Value @'
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup><TargetFramework>net11.0</TargetFramework></PropertyGroup>
  <ItemGroup><Compile Include="Fixture.fs" /></ItemGroup>
</Project>
'@
Set-Content -LiteralPath (Join-Path $fixtureRoot 'fsharp/Fixture.fs') -Value @'
namespace FormattingRegression
type Fixture() =
    member _.Value = 42
'@
Copy-Item -LiteralPath (Join-Path $fixtureRoot 'fsharp/Fixture.fsproj') -Destination (Join-Path $fixtureRoot 'ordering/OrderingOnly.fsproj')
Copy-Item -LiteralPath (Join-Path $fixtureRoot 'fsharp/Fixture.fs') -Destination (Join-Path $fixtureRoot 'ordering/Fixture.fs')
$targets = [System.Security.SecurityElement]::Escape((Join-Path $repositoryRoot 'test/Directory.Build.targets'))
$project = Join-Path $fixtureRoot 'csharp/FormattingRegression.csproj'
Set-Content -LiteralPath $project -Value (@'
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net11.0</TargetFramework>
    <OutputType>Exe</OutputType>
    <GenerateDocumentationFile>true</GenerateDocumentationFile>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include="../fsharp/Fixture.fsproj" />
    <ProjectReference Include="../ordering/OrderingOnly.fsproj" ReferenceOutputAssembly="false" />
  </ItemGroup>
  <Import Project="{0}" />
  <Target Name="AssertOrderingOnlyReference" AfterTargets="ResolveAssemblyReferences" Condition="'$(DesignTimeBuild)' == 'true'">
    <Error Condition="'%(ReferencePath.Filename)' == 'OrderingOnly'" Text="Ordering-only F# output became a compiler reference." />
  </Target>
  <Target Name="AssertMappedFSharpReference" AfterTargets="ResolveAssemblyReferences" Condition="'$(DesignTimeBuild)' == 'true' and '$(Configuration)' == 'Release'">
    <Error Condition="'@(_FSharpDesignTimeAssembly)' == ''" Text="Mapped design-time F# reference was not resolved." />
    <Error Condition="'@(ReferencePath->WithMetadataValue('Filename', 'Fixture'))' == ''" Text="Mapped F# assembly is missing from compiler references." />
  </Target>
</Project>
'@ -f $targets)
$source = Join-Path $fixtureRoot 'csharp/Program.cs'
Set-Content -LiteralPath $source -Value @'
using System.Text;

return new FormattingRegression.Fixture().Value == 42 ? 0 : 1;

internal class AnalyzerFixture
{
    public int GetValue() => 42;
}
'@

Push-Location $fixtureRoot
$previousConfiguration = $env:Configuration
try {
    $env:Configuration = 'Debug'
    $build = Invoke-FormattingCheckDotNet @('run', '--project', $project) 'run.log'
    if ($build.ExitCode -ne 0) { throw "F# runtime reference failed: $($build.Output)" }

    Move-Item -LiteralPath (Join-Path $fixtureRoot 'ordering/bin/Debug/net11.0/OrderingOnly.dll') -Destination (Join-Path $fixtureRoot 'ordering/OrderingOnly.dll.saved')

    $formatArguments = @('format', $project, '--no-restore', '--severity', 'info',
        '--diagnostics', 'IDE0005', 'CA1822', '--verbosity', 'diagnostic', '--include', 'csharp/Program.cs')
    $bothPasses = '(?s)Running Code Style analysis\..*?Running [1-9]\d* analyzers on FormattingRegression\..*?Running Analyzer Reference analysis\..*?Running [1-9]\d* analyzers on FormattingRegression\.'
    $verify = Invoke-FormattingCheckDotNet ($formatArguments + '--verify-no-changes') 'verify-before.log'
    if ($verify.ExitCode -ne 2 -or $verify.Output -notmatch 'IDE0005' -or $verify.Output -notmatch 'CA1822' -or
        $verify.Output -notmatch $bothPasses -or
        $verify.Output -match 'Required references did not load|Msbuild failed') {
        throw "Semantic formatting did not detect both style and analyzer diagnostics (exit $($verify.ExitCode)): $($verify.Output)"
    }
    $apply = Invoke-FormattingCheckDotNet $formatArguments 'apply.log'
    $formattedSource = Get-Content -LiteralPath $source -Raw
    if ($apply.ExitCode -ne 0 -or $formattedSource -match 'using System.Text' -or
        $formattedSource -notmatch 'public static int GetValue\(') {
        throw "Semantic formatting did not apply both style and analyzer fixes: $($apply.Output)"
    }
    $verify = Invoke-FormattingCheckDotNet ($formatArguments + '--verify-no-changes') 'verify-after.log'
    if ($verify.ExitCode -ne 0 -or $verify.Output -notmatch $bothPasses -or
        $verify.Output -match 'Required references did not load|Msbuild failed') {
        throw "Semantic formatting verification failed: $($verify.Output)"
    }

    # Release maps the C# project to Release and both F# projects to Debug.
    $solution = Join-Path $fixtureRoot 'Mapped.sln'
    Set-Content -LiteralPath $solution -Value @'
Microsoft Visual Studio Solution File, Format Version 12.00
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "FormattingRegression", "csharp/FormattingRegression.csproj", "{11111111-1111-1111-1111-111111111111}"
EndProject
Project("{F2A71F9B-5D33-465A-A702-920D77279786}") = "Fixture", "fsharp/Fixture.fsproj", "{22222222-2222-2222-2222-222222222222}"
EndProject
Project("{F2A71F9B-5D33-465A-A702-920D77279786}") = "OrderingOnly", "ordering/OrderingOnly.fsproj", "{33333333-3333-3333-3333-333333333333}"
EndProject
Global
    GlobalSection(SolutionConfigurationPlatforms) = preSolution
        Release|Any CPU = Release|Any CPU
    EndGlobalSection
    GlobalSection(ProjectConfigurationPlatforms) = postSolution
        {11111111-1111-1111-1111-111111111111}.Release|Any CPU.ActiveCfg = Release|Any CPU
        {11111111-1111-1111-1111-111111111111}.Release|Any CPU.Build.0 = Release|Any CPU
        {22222222-2222-2222-2222-222222222222}.Release|Any CPU.ActiveCfg = Debug|Any CPU
        {22222222-2222-2222-2222-222222222222}.Release|Any CPU.Build.0 = Debug|Any CPU
        {33333333-3333-3333-3333-333333333333}.Release|Any CPU.ActiveCfg = Debug|Any CPU
        {33333333-3333-3333-3333-333333333333}.Release|Any CPU.Build.0 = Debug|Any CPU
    EndGlobalSection
EndGlobal
'@
    $mappedBuild = Invoke-FormattingCheckDotNet @('build', $solution, '-c', 'Release') 'mapped-build.log'
    if ($mappedBuild.ExitCode -ne 0) { throw "Mapped solution build failed: $($mappedBuild.Output)" }
    # The pinned formatter does not forward solution mappings. Exercise the design-time
    # target with MSBuild's real solution context, which supplies SetConfiguration.
    $mapped = Invoke-FormattingCheckDotNet @('msbuild', $solution, '-t:ResolveReferences',
        '-p:DesignTimeBuild=true', '-p:Configuration=Release') 'mapped-design-time.log'
    if ($mapped.ExitCode -ne 0) { throw "Mapped design-time reference resolution failed: $($mapped.Output)" }

    $assembly = Join-Path $fixtureRoot 'fsharp/bin/Debug/net11.0/Fixture.dll'
    Move-Item -LiteralPath $assembly -Destination "$assembly.saved"
    try {
        $missing = Invoke-FormattingCheckDotNet ($formatArguments + '--verify-no-changes') 'missing-assembly.log'
        if ($missing.Output -notmatch 'Build the affected solution with Configuration=Debug before semantic formatting. Missing F# assembly:') {
            throw "Missing F# build output was not explained: $($missing.Output)"
        }
    }
    finally {
        Move-Item -LiteralPath "$assembly.saved" -Destination $assembly
    }

    # Empty conversion results are valid for ordering-only references and for C#-only projects.
    [xml]$projectXml = Get-Content -LiteralPath $project
    $references = @($projectXml.Project.ItemGroup.ProjectReference)
    foreach ($reference in $references) {
        $reference.ParentNode.RemoveChild($reference) | Out-Null
        $projectXml.Save($project)
        $label = if ($reference.Include -like '*Fixture.fsproj') { 'ordering-only' } else { 'no-fsharp' }
        $resolve = Invoke-FormattingCheckDotNet @('msbuild', $project, '-t:ResolveReferences',
            '-p:DesignTimeBuild=true') "$label.log"
        if ($resolve.ExitCode -ne 0) { throw "Reference resolution failed for ${label}: $($resolve.Output)" }
    }

    Write-Output "F# runtime reference and semantic formatting regression passed. Evidence: $fixtureRoot"
}
finally {
    $env:Configuration = $previousConfiguration
    Pop-Location
}
