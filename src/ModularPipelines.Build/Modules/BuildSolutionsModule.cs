using EnumerableAsyncProcessor.Extensions;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Helpers;
using ModularPipelines.Build.Modules.UnitTests;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

[RequiresCapability("ci-master")]
[RunIf<ModularPipelines.OnLinux>]
[ProducesArtifact("build-output", "../../_build-output.tar.gz")]
public class BuildSolutionsModule(IOptions<PipelineSettings> pipelineSettings) : Module<CommandResult[]>
{
    protected override async Task<CommandResult[]> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var repositoryInfo = await context.Tools.Git.Information.GetInfoAsync(cancellationToken).ConfigureAwait(false)
            ?? throw new InvalidOperationException("Git repository information is unavailable.");
        var gitRoot = repositoryInfo.Root.Path;
        // Standalone CI prebuilds solutions before starting the host. Distributed CI
        // builds here so dependent workers can consume the published outputs.
        CommandResult[] results = [];
        if (!pipelineSettings.Value.BuildAlreadyCompleted)
        {
            var solutions = File.ReadLines(Path.Combine(gitRoot, "BuildSolutions.txt"))
                .Select(line => line.Trim())
                .Where(line => !string.IsNullOrEmpty(line) && !line.StartsWith('#'))
                .ToArray();

            results = await solutions
                .ToAsyncProcessorBuilder()
                .SelectAsync(async solution => await context.Tools.DotNet.BuildAsync(new DotNetBuildOptions
                {
                    ProjectSolution = Path.Combine(gitRoot, solution),
                    Configuration = "Release",
                    NoRestore = true,
                }, cancellationToken: cancellationToken))
                .ProcessOneAtATime();
        }

        if (!context.Services.GetRequiredService<BuildOutputSharing>().IsEnabled)
        {
            return results;
        }

        var testProjects = context.Services.GetRequiredService<IEnumerable<IModule>>()
            .OfType<RunUnitTestModule>()
            .Select(module => module.BuildOutputProjectFileName);
        await BuildOutputArchive.CreateAsync(gitRoot, testProjects, cancellationToken).ConfigureAwait(false);
        return results;
    }
}
