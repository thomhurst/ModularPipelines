using System.Text.Json;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;
using ModularPipelines.TestHelpers.Assertions;

namespace ModularPipelines.DotNet.UnitTests;

public class DotNetTests : TestBase
{
    private class DotNetPackageListModule : Module<CommandResult>
    {
        protected internal override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var directory = Directory.CreateTempSubdirectory("ModularPipelines package list ");
            try
            {
                var project = Path.Combine(directory.FullName, "PackageListFixture.csproj");
                await File.WriteAllTextAsync(project, """
                    <Project Sdk="Microsoft.NET.Sdk">
                      <PropertyGroup>
                        <TargetFramework>net11.0</TargetFramework>
                        <NuGetAudit>false</NuGetAudit>
                      </PropertyGroup>
                    </Project>
                    """, cancellationToken);

                var nugetConfig = Path.Combine(directory.FullName, "NuGet.Config");
                await File.WriteAllTextAsync(nugetConfig, """
                    <configuration>
                      <packageSources>
                        <clear />
                      </packageSources>
                    </configuration>
                    """, cancellationToken);

                var executionOptions = new CommandExecutionOptions
                {
                    // Resolve the repository's pinned SDK while keeping the fixture outside its build imports.
                    WorkingDirectory = AppContext.BaseDirectory,
                    ExecutionTimeout = TimeSpan.FromSeconds(30),
                };

                await context.Tools.DotNet.RestoreAsync(new DotNetRestoreOptions
                {
                    ProjectSolution = project,
                    Configfile = nugetConfig,
                    DisableBuildServers = true,
                }, executionOptions, cancellationToken);

                return await context.Tools.DotNet.Package.ListAsync(new DotNetPackageListOptions
                {
                    Project = project,
                    NoRestore = true,
                    Format = "json",
                }, executionOptions, cancellationToken);
            }
            finally
            {
                directory.Delete(recursive: true);
            }
        }
    }

    [Test]
    public async Task Lists_Packages_For_A_Minimal_Restored_Project()
    {
        var moduleResult = await await RunModule<DotNetPackageListModule>(new TestHostSettings
        {
            TestTimeout = TimeSpan.FromMinutes(1),
        });

        await ModuleResultAssertions.AssertSuccessWithValue(moduleResult);
        await Assert.That(moduleResult.Value.ExitCode).IsEqualTo(0);

        using var output = JsonDocument.Parse(moduleResult.Value.StandardOutput);
        var projects = output.RootElement.GetProperty("projects");
        await Assert.That(projects.GetArrayLength()).IsEqualTo(1);
        await Assert.That(Path.GetFileName(projects[0].GetProperty("path").GetString()))
            .IsEqualTo("PackageListFixture.csproj");
        var frameworks = projects[0].GetProperty("frameworks");
        await Assert.That(frameworks.GetArrayLength()).IsEqualTo(1);
        await Assert.That(frameworks[0].GetProperty("framework").GetString()).IsEqualTo("net11.0");
    }
}
