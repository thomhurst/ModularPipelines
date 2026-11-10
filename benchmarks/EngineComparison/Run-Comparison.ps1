#!/usr/bin/env pwsh
<#
.SYNOPSIS
Compares ModularPipelines 3.2.8 with the current source on one machine.
.DESCRIPTION
Run from an isolated checkout using the SDK in global.json. Requires PowerShell 7
and NuGet access. All builds and worker launches use Invoke-AgentDotNet.ps1.
The script generates two standalone projects per scenario/count outside the checkout.
Each binary contains exactly N concrete module types, so unrelated scenarios do
not distort assembly scanning, generated metadata or peak memory. Both versions target
the same runtime; only the framework reference and API adapter differ. No BDN
dependency is needed: the operation is a complete asynchronous pipeline lifecycle.

Each fresh worker warms its scenario once, forces GC, then measures builder
creation, registration, validation, execution and async disposal. Tiered JIT is
disabled for both versions. Repetitions alternate the version order. The fixed
module limit is eight. Console output and v4 run-history/report writes are disabled.
Build/run/disposal times are also retained to expose fixed shutdown delays.
Each command module starts one short native process:
cmd.exe /d /c exit 0 on Windows, /bin/sh -c 'exit 0' on Linux.

Wall time excludes process startup and warmup. Allocations use process-wide
GC.GetTotalAllocatedBytes(true), so asynchronous work is included; native and
child-process allocations are excluded. Peak working set is the OS high-water
mark for the worker's entire lifetime, including warmup and JIT, excluding child
processes. Do not interpret it as incremental retained heap or tree-wide RSS.

summary.md reports median [min, max], paired ratios and candidates exceeding 20%
in every paired sample. Candidates require a second independent run before filing
regressions. samples.json retains raw data and machine/runtime/source metadata.
SourceDirty covers the core, generators, build settings and benchmark inputs.
Compare versions only within the same OS/machine/run, never Windows against Linux.
Use an idle machine. Container measurements must identify image and resource limits.
.EXAMPLE
& benchmarks/EngineComparison/Run-Comparison.ps1 `
  -OutputDirectory C:/git/ModularPipelines-evidence/issue-7322/windows
.EXAMPLE
# Smoke validation, not release evidence:
& benchmarks/EngineComparison/Run-Comparison.ps1 `
  -OutputDirectory C:/git/ModularPipelines-evidence/issue-7322/smoke `
  -Counts 2 -Repetitions 1
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [string]$OutputDirectory,
    [ValidateRange(1, 500)]
    [int[]]$Counts = @(50, 500),
    [ValidateRange(1, 30)]
    [int]$Repetitions = 5,
    [ValidateSet('independent', 'chain', 'fanout', 'commands')]
    [string[]]$Shapes = @('independent', 'chain', 'fanout', 'commands'),
    [string]$EnvironmentDescription = 'Native host; no harness CPU or memory quota beyond the dotnet guard'
)

$ErrorActionPreference = 'Stop'
$repo = (Resolve-Path (Join-Path $PSScriptRoot '../..')).Path
$guard = Join-Path $repo 'scripts/Invoke-AgentDotNet.ps1'
$OutputDirectory = [IO.Path]::GetFullPath($OutputDirectory)
if (Test-Path (Join-Path $OutputDirectory 'samples.json')) {
    throw 'Choose a new output directory to preserve earlier measurements.'
}
New-Item -ItemType Directory -Force $OutputDirectory | Out-Null
$sourceCommit = git -C $repo rev-parse HEAD
if ($LASTEXITCODE -ne 0) { throw 'Cannot identify source commit.' }
$inputPaths = @('global.json', '*.props', '*.targets', 'NuGet.Config', 'nuget.config',
    'src/ModularPipelines', 'src/ModularPipelines.SourceGenerator',
    'src/ModularPipelines.Development.Analyzers', 'src/ModularPipelines.Analyzers',
    'benchmarks/EngineComparison', 'scripts/Invoke-AgentDotNet.ps1')
$sourceDirty = @(git -C $repo status --porcelain --untracked-files=normal -- $inputPaths).Count -gt 0
if ($LASTEXITCODE -ne 0) { throw 'Cannot check benchmark input state.' }
$sdk = (Get-Content (Join-Path $repo 'global.json') -Raw | ConvertFrom-Json).sdk.version
$framework = 'net' + ($sdk -split '\.')[0] + '.0'
Push-Location $repo
try {
    & $guard -LogPath (Join-Path $OutputDirectory 'sdk-version.log') -DotNetArguments @('--version')
    if ($LASTEXITCODE -ne 0) { throw "SDK detection failed with exit $LASTEXITCODE." }
}
finally { Pop-Location }
$resolvedSdk = (Get-Content (Join-Path $OutputDirectory 'sdk-version.log') -Raw).Trim()
function Build-Worker([string]$Shape, [int]$Count, [string]$Version) {
    $moduleSource = [Text.StringBuilder]::new()
    [void]$moduleSource.AppendLine(@'
using ModularPipelines;
#if LEGACY
using ModularPipelines.Extensions;
using ModularPipelines.Attributes;
#endif
namespace EngineComparison;
'@)
    $registrationSource = [Text.StringBuilder]::new()
    [void]$registrationSource.AppendLine('internal static class Scenarios { internal static readonly Dictionary<string, Action<PipelineBuilder>[]> Registrations = new() {')
    [void]$registrationSource.AppendLine("[`"$Shape`"] = [")
    for ($i = 0; $i -lt $Count; $i++) {
        $name = "$($Shape)Module$i"
        $predecessor = -1
        if ($Shape -eq 'chain' -and $i -gt 0) { $predecessor = $i - 1 }
        if ($Shape -eq 'fanout' -and $i -gt 0) { $predecessor = 0 }
        if ($predecessor -ge 0) {
            [void]$moduleSource.AppendLine("[DependsOn<$($Shape)Module$predecessor>]")
        }
        [void]$moduleSource.AppendLine("public sealed class $name() : TrivialModule($i, $predecessor);")
        [void]$registrationSource.AppendLine("static builder => builder.AddModule<$name>(),")
    }
    [void]$registrationSource.AppendLine('], }; }')
    [void]$moduleSource.Append($registrationSource)
    $directory = Join-Path $OutputDirectory "$Shape-$Count/$Version"
    $projectName = "Worker_${Shape}_${Count}"
    $projectPath = Join-Path $directory "$projectName.csproj"
    $artifacts = Join-Path $OutputDirectory "$Version/artifacts"
    New-Item -ItemType Directory -Force $directory | Out-Null
    Copy-Item (Join-Path $PSScriptRoot 'Worker.cs') $directory
    [IO.File]::WriteAllText((Join-Path $directory 'Scenarios.cs'), $moduleSource.ToString())
    $reference = '<PackageReference Include="ModularPipelines" Version="[3.2.8]" />'
    $define = 'LEGACY'
    if ($Version -eq 'v4') {
        $escapedPath = [Security.SecurityElement]::Escape((Join-Path $repo 'src/ModularPipelines/ModularPipelines.csproj'))
        $generatorPath = [Security.SecurityElement]::Escape((Join-Path $repo 'src/ModularPipelines.SourceGenerator/ModularPipelines.SourceGenerator.csproj'))
        $reference = "<ProjectReference Include=`"$escapedPath`" /><ProjectReference Include=`"$generatorPath`" ReferenceOutputAssembly=`"false`" OutputItemType=`"Analyzer`" SetTargetFramework=`"TargetFramework=netstandard2.0`" />"
        $define = ''
    }
    @"
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>$framework</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <LangVersion>preview</LangVersion>
    <DefineConstants>$define</DefineConstants>
    <ManagePackageVersionsCentrally>false</ManagePackageVersionsCentrally>
    <IsPackable>false</IsPackable>
  </PropertyGroup>
  <ItemGroup>$reference</ItemGroup>
</Project>
"@ | Set-Content $projectPath
    # Resolve the pinned SDK even though the generated project is outside the checkout.
    Push-Location $repo
    try {
        & $guard -SingleNode -LogPath (Join-Path $OutputDirectory "build-$Shape-$Count-$Version.log") `
            -DotNetArguments @('build', $projectPath, '-c', 'Release', '--artifacts-path', $artifacts, '-p:RunAnalyzers=false')
        if ($LASTEXITCODE -ne 0) { throw "Build $Shape-$Count-$Version failed with exit $LASTEXITCODE. Limits are not retried." }
    }
    finally { Pop-Location }
    return Join-Path $artifacts "bin/$projectName/release/$projectName.dll"
}

$previousTiering = $env:DOTNET_TieredCompilation
$samples = [Collections.Generic.List[object]]::new()
try {
    $env:DOTNET_TieredCompilation = '0'
    foreach ($shape in $Shapes) {
        foreach ($count in $Counts) {
            $workers = @{}
            foreach ($version in @('v3', 'v4')) {
                $workers[$version] = Build-Worker $shape $count $version
            }
            for ($repeat = 1; $repeat -le $Repetitions; $repeat++) {
                $versions = @('v3', 'v4')
                if ($repeat % 2 -eq 0) { $versions = @('v4', 'v3') }
                foreach ($version in $versions) {
                    $name = "$shape-$count-$repeat-$version"
                    $resultPath = Join-Path $OutputDirectory "$name.json"
                    $dll = $workers[$version]
                    Push-Location $repo
                    try {
                        & $guard -TimeoutSeconds 180 -LogPath (Join-Path $OutputDirectory "$name.log") `
                            -DotNetArguments @($dll, $shape, "$count", $resultPath)
                        if ($LASTEXITCODE -ne 0) { throw "Sample $name failed with exit $LASTEXITCODE. Limits are not retried." }
                    }
                    finally { Pop-Location }
                    $sample = Get-Content $resultPath -Raw | ConvertFrom-Json
                    $sample | Add-Member Version $version
                    $sample | Add-Member Repeat $repeat
                    $samples.Add($sample)
                    Write-Host "$name complete"
                }
            }
        }
    }
}
finally {
    $env:DOTNET_TieredCompilation = $previousTiering
    $cpu = if ($IsWindows) { (Get-CimInstance Win32_Processor | Select-Object -First 1).Name } else { (Get-Content /proc/cpuinfo | Select-String '^model name' | Select-Object -First 1).ToString() }
    [ordered]@{
        SourceCommit = $sourceCommit
        SourceDirty = $sourceDirty
        BaselinePackage = '3.2.8'
        SDK = $sdk
        ResolvedSDK = $resolvedSdk
        Environment = $EnvironmentDescription
        CPU = $cpu
        Machine = [Environment]::MachineName
        TimestampUtc = [DateTime]::UtcNow.ToString('O')
        TieredCompilation = '0'
        Samples = @($samples.ToArray())
    } | ConvertTo-Json -Depth 8 | Set-Content (Join-Path $OutputDirectory 'samples.json')
}

function Get-Median([double[]]$Values) {
    $sorted = @($Values | Sort-Object)
    $middle = [int][Math]::Floor($sorted.Count / 2)
    if ($sorted.Count % 2 -eq 1) { return $sorted[$middle] }
    return ($sorted[$middle - 1] + $sorted[$middle]) / 2
}

function Format-Range([double[]]$Values) {
    $range = $Values | Measure-Object -Minimum -Maximum
    return '{0:F2} [{1:F2}, {2:F2}]' -f (Get-Median $Values), $range.Minimum, $range.Maximum
}

$report = [Collections.Generic.List[string]]::new()
$report.Add("Source: $sourceCommit (dirty: $sourceDirty). Baseline: NuGet 3.2.8. SDK: $resolvedSdk (global.json: $sdk). CPU: $cpu.")
$report.Add("Environment: $EnvironmentDescription")
$report.Add('')
$report.Add('Values: median [min, max]. Ratios: v4/v3 for paired fresh-process samples. Wall time in ms; memory and allocations in bytes.')
$report.Add('Peak working set includes worker startup, JIT and warmup; excludes child processes. Allocations exclude native and child-process allocations.')
$report.Add('')
$report.Add('| Scenario | N | Metric | v3 | v4 | Paired ratio median [min, max] | >20% in every pair |')
$report.Add('|---|---:|---|---:|---:|---:|---|')
foreach ($shape in $Shapes) {
    foreach ($count in $Counts) {
        $group = @($samples | Where-Object { $_.Shape -eq $shape -and $_.Count -eq $count })
        foreach ($metric in @('WallMilliseconds', 'PeakWorkingSetBytes', 'AllocatedBytes')) {
            $baseline = @($group | Where-Object Version -eq 'v3' | Sort-Object Repeat | ForEach-Object { $_.$metric })
            $current = @($group | Where-Object Version -eq 'v4' | Sort-Object Repeat | ForEach-Object { $_.$metric })
            $ratios = @(for ($i = 0; $i -lt $baseline.Count; $i++) { $current[$i] / $baseline[$i] })
            $cells = @((Format-Range $baseline), (Format-Range $current), (Format-Range $ratios))
            $candidate = $Repetitions -ge 3 -and @($ratios | Where-Object { $_ -le 1.2 }).Count -eq 0
            $report.Add("| $shape | $count | $metric | $($cells -join ' | ') | $candidate |")
        }
    }
}
$report | Set-Content (Join-Path $OutputDirectory 'summary.md')
Write-Host "Results: $(Join-Path $OutputDirectory 'summary.md')"
