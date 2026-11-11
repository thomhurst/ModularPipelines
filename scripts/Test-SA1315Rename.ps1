#!/usr/bin/env pwsh
# Reproduces upstream rename binding loss; a passing run confirms the defect and workaround.
param(
    [string]$DotNetPath = 'dotnet',
    [string]$ArtifactsPath
)

$ErrorActionPreference = 'Stop'
$repo = Split-Path $PSScriptRoot -Parent
if (-not $ArtifactsPath) {
    $commonGitDirectory = git -C $repo rev-parse --path-format=absolute --git-common-dir
    if ($LASTEXITCODE -ne 0) { throw 'Cannot locate shared checkout evidence directory' }
    $sharedRepo = Split-Path $commonGitDirectory -Parent
    $ArtifactsPath = Join-Path (Split-Path $sharedRepo -Parent) 'ModularPipelines-evidence/issue-7464'
}
$fixture = Join-Path ([IO.Path]::GetFullPath($ArtifactsPath)) ("sa1315-{0}" -f [guid]::NewGuid())
New-Item -ItemType Directory -Path $fixture, "$fixture/rename" | Out-Null
Copy-Item (Join-Path $repo 'global.json') "$fixture/global.json"
# The fixture must not inherit repository build props, targets or package versions.
Set-Content "$fixture/Directory.Build.props" '<Project />'
Set-Content "$fixture/Directory.Build.targets" '<Project />'
Set-Content "$fixture/Directory.Packages.props" '<Project />'
Set-Content "$fixture/.editorconfig" "root = true`n[*.cs]`ndotnet_diagnostic.SA1315.severity = warning"
Set-Content "$fixture/Repro.csproj" @'
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net11.0</TargetFramework>
    <LangVersion>preview</LangVersion>
  </PropertyGroup>
  <ItemGroup>
    <Compile Remove="rename/**/*.cs" />
    <PackageReference Include="StyleCop.Analyzers" Version="1.3.180" PrivateAssets="all" />
  </ItemGroup>
</Project>
'@
$source = @'
using System;

public interface ILogger
{
    void Log(Func<string> formatter);
}

public interface IFormatter
{
    string GetLogIssueCommand();
}

public sealed class Logger(IFormatter formatter) : ILogger
{
    public void Log(Func<string> messageFormatter)
    {
        Console.WriteLine(formatter.GetLogIssueCommand());
        Console.WriteLine(messageFormatter());
    }
}

'@
Set-Content "$fixture/Logger.cs" $source
# Use exactly the Roslyn assemblies hosted by the selected SDK's formatter.
Set-Content "$fixture/rename/Rename.csproj" @'
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net11.0</TargetFramework>
    <OutputType>Exe</OutputType>
  </PropertyGroup>
  <ItemGroup>
    <Reference Include="$(MSBuildSDKsPath)/../DotnetTools/dotnet-format/*.dll" />
  </ItemGroup>
</Project>
'@
Set-Content "$fixture/rename/Program.cs" @'
using System;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Text;

using var workspace = new AdhocWorkspace();
var project = workspace.AddProject("Repro", LanguageNames.CSharp)
    .WithCompilationOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
    .WithParseOptions(new CSharpParseOptions(LanguageVersion.Preview))
    .AddMetadataReferences(((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")).Split(Path.PathSeparator).Select(p => MetadataReference.CreateFromFile(p)));
var document = project.AddDocument("Logger.cs", SourceText.From(File.ReadAllText(args[0])));
var root = await document.GetSyntaxRootAsync();
var model = await document.GetSemanticModelAsync();
var parameter = root.DescendantNodes().OfType<ParameterSyntax>().Single(p => p.Identifier.ValueText == "messageFormatter");
var symbol = model.GetDeclaredSymbol(parameter);
Console.WriteLine(typeof(Renamer).Assembly.FullName);
var solution = await Renamer.RenameSymbolAsync(document.Project.Solution, symbol, new SymbolRenameOptions(), "formatter");
var renamed = solution.GetDocument(document.Id);
Console.WriteLine(await renamed.GetTextAsync());
var renamedRoot = await renamed.GetSyntaxRootAsync();
Console.WriteLine("Conflict annotations: " + renamedRoot.GetAnnotatedNodesAndTokens(Microsoft.CodeAnalysis.CodeActions.ConflictAnnotation.Kind).Count());
var compilation = await renamed.Project.GetCompilationAsync();
var errors = compilation.GetDiagnostics().Where(d => d.Severity == DiagnosticSeverity.Error).ToArray();
foreach (var error in errors) Console.WriteLine(error);
return errors.Any(d => d.Id == "CS1061") ? 0 : 1;

'@

function Invoke-Check([string[]]$Arguments, [string]$Name, [int]$ExpectedExit = 0) {
    $log = Join-Path $fixture "$Name.log"
    & (Join-Path $repo 'scripts/Invoke-AgentDotNet.ps1') -DotNetPath $DotNetPath `
        -SingleNode -LogPath $log -DotNetArguments $Arguments
    $code = $LASTEXITCODE
    if ($code -in 124, 137) { throw "Validation limit reached ($code); defer remaining validation to CI. See $log" }
    if ($code -ne $ExpectedExit) { throw "Expected exit $ExpectedExit, got $code. See $log" }
    return Get-Content -LiteralPath $log -Raw
}

Push-Location $fixture
try {
    $null = Invoke-Check @('--version') 'sdk'
    $baseline = Invoke-Check @('build', 'Repro.csproj') 'before'
    if ($baseline -notmatch 'warning SA1315:') { throw 'Baseline did not report SA1315' }
    $null = Invoke-Check @('run', '--project', 'rename/Rename.csproj', '--', "$fixture/Logger.cs") 'direct-rename'
    $formatArgs = @('format', 'Repro.csproj', '--no-restore', '--diagnostics', 'SA1315', '--include', 'Logger.cs', '--verbosity', 'diagnostic')
    $format = Invoke-Check $formatArgs 'format'
    if ($format -match 'Required references did not load|Msbuild failed' -or
        (Get-Content "$fixture/Logger.cs" -Raw) -notmatch 'public void Log\(Func<string> formatter\)') {
        throw "Formatter did not apply SA1315. Check SDK/MSBuild installation selection in $fixture/format.log"
    }
    $broken = Invoke-Check @('build', 'Repro.csproj', '--no-restore') 'after' 1
    if ($broken -notmatch 'CS1061') { throw 'Expected binding error CS1061 was not reproduced' }
    Copy-Item "$fixture/Logger.cs" "$fixture/Logger.broken.txt"
    # Rename the captured constructor parameter AND its reference before applying SA1315.
    $safe = $source.Replace('Logger(IFormatter formatter)', 'Logger(IFormatter buildSystemFormatter)').Replace(
        'formatter.GetLogIssueCommand()', 'buildSystemFormatter.GetLogIssueCommand()')
    Set-Content "$fixture/Logger.cs" $safe
    $null = Invoke-Check $formatArgs 'safe-format'
    $safeBuild = Invoke-Check @('build', 'Repro.csproj', '--no-restore') 'safe-build'
    if ($safeBuild -match 'warning SA1315:' -or
        (Get-Content "$fixture/Logger.cs" -Raw) -notmatch 'public void Log\(Func<string> formatter\)') {
        throw 'Safe cleanup did not eliminate SA1315'
    }
    Write-Host "Confirmed SA1315 and direct Roslyn binding failure; safe rename compiles. Evidence: $fixture"
}
finally {
    Pop-Location
}
