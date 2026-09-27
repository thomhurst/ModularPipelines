using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Modules.UnitTests;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Build.Modules;

/// <summary>
/// Builds and tests the core library on a non-Linux platform.
/// </summary>
/// <remarks>
/// Linux builds every solution and runs every test project. Other platforms only need to
/// prove that the core library compiles and passes its tests there; compiling the large
/// generated integrations on these runners dominated the distributed critical path.
/// </remarks>
public abstract class BuildAndTestCoreOnPlatformModule(IOptions<PipelineSettings> pipelineSettings) : Module<CommandResult[]>
{
    private const string CoreTestSolution = "ModularPipelines.Tests.slnf";
    private const string CoreTestProject = "test/ModularPipelines.UnitTests/ModularPipelines.UnitTests.csproj";

    // The same budget applies to commands and the enclosing module, leaving
    // 15 minutes below the job timeout.
    private static readonly TimeSpan BuildTimeout = TimeSpan.FromMinutes(75);

    protected override void Configure(ModuleConfigurationBuilder module) => module
        .WithTimeout(BuildTimeout);

    protected override async Task<CommandResult[]> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var repositoryInfo = await context.Tools.Git.Information.GetInfoAsync(cancellationToken).ConfigureAwait(false)
            ?? throw new InvalidOperationException("Git repository information is unavailable.");

        var build = await context.Tools.DotNet.BuildAsync(new DotNetBuildOptions
        {
            ProjectSolution = Path.Combine(repositoryInfo.Root.Path, CoreTestSolution),
            Configuration = "Release",
            Arguments = ["-p:UseSharedCompilation=false"],
        }, new CommandExecutionOptions
        {
            ExecutionTimeout = BuildTimeout,
        }, cancellationToken: cancellationToken).ConfigureAwait(false);

        // Coverage is collected on Linux; platform runs only need the test outcome.
        var test = await RunUnitTestModule.RunTestProjectAsync(
            context,
            repositoryInfo.Root.GetFile(CoreTestProject),
            pipelineSettings.Value.TestFramework,
            [],
            cancellationToken).ConfigureAwait(false);

        return [build, test];
    }
}

[RunIf<ModularPipelines.OnWindows>]
public sealed class BuildAndTestCoreOnWindowsModule(IOptions<PipelineSettings> pipelineSettings)
    : BuildAndTestCoreOnPlatformModule(pipelineSettings)
{
}

[RunIf<ModularPipelines.OnMacOS>]
public sealed class BuildAndTestCoreOnMacOSModule(IOptions<PipelineSettings> pipelineSettings)
    : BuildAndTestCoreOnPlatformModule(pipelineSettings)
{
}
