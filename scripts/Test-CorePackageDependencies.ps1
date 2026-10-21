param([string]$DotNetPath = 'dotnet')

$ErrorActionPreference = 'Stop'
$repositoryRoot = Split-Path $PSScriptRoot -Parent
$testRoot = Join-Path ([System.IO.Path]::GetTempPath()) ("core-package-dependencies-{0}" -f [guid]::NewGuid())
$targets = [System.Security.SecurityElement]::Escape((Join-Path $repositoryRoot 'src/Directory.Build.targets'))
$guard = Join-Path $PSScriptRoot 'Invoke-AgentDotNet.ps1'

try {
    New-Item -ItemType Directory -Path $testRoot | Out-Null
    # Keep SDK selection reproducible even though fixtures live outside the checkout.
    Copy-Item -LiteralPath (Join-Path $repositoryRoot 'global.json') -Destination $testRoot
    New-Item -ItemType Directory -Path (Join-Path $testRoot 'core'), (Join-Path $testRoot 'other') | Out-Null
    $coreProject = Join-Path $testRoot 'core/ModularPipelines.csproj'
    Set-Content -LiteralPath $coreProject -Value @'
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net11.0</TargetFramework>
    <EnableDefaultItems>false</EnableDefaultItems>
    <PackageVersion>4.0.1</PackageVersion>
  </PropertyGroup>
</Project>
'@
    Set-Content -LiteralPath (Join-Path $testRoot 'other/OtherDependency.csproj') -Value @'
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net11.0</TargetFramework>
    <EnableDefaultItems>false</EnableDefaultItems>
    <PackageVersion>2.3.4</PackageVersion>
  </PropertyGroup>
</Project>
'@

    $core = [xml](Get-Content -LiteralPath (Join-Path $repositoryRoot 'src/ModularPipelines/ModularPipelines.csproj') -Raw)
    $friends = @($core.SelectNodes('//AssemblyAttribute[@Include="System.Runtime.CompilerServices.InternalsVisibleToAttribute"]/_Parameter1') |
        ForEach-Object InnerText | Sort-Object -Unique)
    $packages = @(Get-Content -LiteralPath (Join-Path $repositoryRoot 'PackProjects.txt') |
        Where-Object { $_ -match '^src/' } |
        ForEach-Object { [System.IO.Path]::GetFileNameWithoutExtension($_) })
    $shippedFriends = @($packages | Where-Object { $_ -in $friends })
    if ($shippedFriends.Count -eq 0) { throw 'No shipped core friends found.' }

    # A public-only package must keep its open range. Include every shipped friend so
    # adding another InternalsVisibleTo declaration automatically expands coverage.
    $assemblies = @($shippedFriends) + 'ModularPipelines.Cmd'
    $checks = @()
    $expectations = @()
    foreach ($assembly in $assemblies) {
        foreach ($scenario in 'stable', 'prerelease', 'beta', 'direct') {
            $coreVersion = if ($scenario -eq 'prerelease') { '4.0.1-alpha0001' } else { '4.0.1' }
            $packageVersion = if ($scenario -eq 'beta') { '4.0.1-beta' } else { $coreVersion }
            $directory = Join-Path $testRoot "$assembly-$scenario"
            New-Item -ItemType Directory -Path $directory | Out-Null
            Set-Content -LiteralPath (Join-Path $directory 'content.txt') -Value 'Pack dependency fixture.'
            $project = Join-Path $directory 'Fixture.csproj'
            Set-Content -LiteralPath $project -Value (@'
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net11.0</TargetFramework>
    <AssemblyName>{0}</AssemblyName>
    <PackageId>{0}</PackageId>
    <PackageVersion>{1}</PackageVersion>
    <EnableDefaultItems>false</EnableDefaultItems>
    <IncludeBuildOutput>false</IncludeBuildOutput>
    <NoPackageAnalysis>true</NoPackageAnalysis>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include="../core/ModularPipelines.csproj" />
    <ProjectReference Include="../other/OtherDependency.csproj" />
    <None Include="content.txt" Pack="true" PackagePath="content" />
  </ItemGroup>
  <Import Project="{2}" />
</Project>
'@ -f $assembly, $packageVersion, $targets)
            # Match PackProjectsModule's global version properties, including the
            # unsuffixed core release for beta packages. Direct pack has no override.
            $properties = if ($scenario -eq 'direct') { '' } else {
                "PackageVersion=$packageVersion;Version=$packageVersion;ModularPipelinesCorePackageVersion=$coreVersion"
            }
            $checks += '<MSBuild Projects="{0}" Targets="Restore;Pack" Properties="Configuration=Release;NoBuild=true;{1}" />' -f
                [System.Security.SecurityElement]::Escape($project), $properties
            $expectations += @{
                Directory = $directory
                Assembly = $assembly
                PackageVersion = $packageVersion
                CoreVersion = $coreVersion
                OtherVersion = if ($scenario -eq 'direct') { '2.3.4' } else { $packageVersion }
                Exact = $assembly -in $shippedFriends
            }
        }
    }
    $driver = Join-Path $testRoot 'Tests.proj'
    Set-Content -LiteralPath $driver -Value ('<Project><Target Name="Test">' + ($checks -join "`n") + '</Target></Project>')
    # The guard exits its PowerShell host, so use a child process to retain finally.
    $command = "& '{0}' -DotNetPath '{1}' -SingleNode -TimeoutSeconds 120 -DotNetArguments @('msbuild', '{2}', '-target:Test', '-verbosity:minimal')" -f
        $guard.Replace("'", "''"), $DotNetPath.Replace("'", "''"), $driver.Replace("'", "''")
    $output = @(& pwsh -NoProfile -Command $command 2>&1)
    if ($LASTEXITCODE -ne 0) { throw "Pack fixtures failed (exit $LASTEXITCODE): $($output -join "`n")" }

    foreach ($expected in $expectations) {
        $package = Join-Path $expected.Directory "bin/Release/$($expected.Assembly).$($expected.PackageVersion).nupkg"
        $archive = [System.IO.Compression.ZipFile]::OpenRead($package)
        try {
            $nuspec = @($archive.Entries | Where-Object { $_.FullName.EndsWith('.nuspec', [StringComparison]::Ordinal) })
            if ($nuspec.Count -ne 1) { throw "Expected one nuspec in $package." }
            $reader = [System.IO.StreamReader]::new($nuspec[0].Open())
            try { $xml = [xml]$reader.ReadToEnd() } finally { $reader.Dispose() }
            $dependency = $xml.SelectSingleNode('//*[local-name()="dependency" and @id="ModularPipelines"]')
            $range = if ($expected.Exact) { "[$($expected.CoreVersion)]" } else { $expected.CoreVersion }
            if ($null -eq $dependency -or $dependency.version -cne $range) {
                throw "${package}: expected core dependency '$range', got '$($dependency.version)'."
            }
            $other = $xml.SelectSingleNode('//*[local-name()="dependency" and @id="OtherDependency"]')
            if ($null -eq $other -or $other.version -cne $expected.OtherVersion) {
                throw "${package}: unrelated dependency changed to '$($other.version)'."
            }
        }
        finally { $archive.Dispose() }
    }
    Write-Output "Core package dependencies passed ($($expectations.Count) packed nuspecs)."
}
finally {
    $resolvedRoot = [System.IO.Path]::GetFullPath($testRoot)
    $temporaryRoot = [System.IO.Path]::GetFullPath([System.IO.Path]::GetTempPath())
    if (-not $resolvedRoot.StartsWith($temporaryRoot, [StringComparison]::OrdinalIgnoreCase)) {
        throw "Refusing cleanup outside the temporary directory: $resolvedRoot"
    }
    if (Test-Path -LiteralPath $resolvedRoot) { Remove-Item -LiteralPath $resolvedRoot -Recurse -Force }
}
