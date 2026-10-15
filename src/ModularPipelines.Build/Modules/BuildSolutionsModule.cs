using EnumerableAsyncProcessor.Extensions;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Helpers;
using ModularPipelines.Build.Modules.UnitTests;
using ModularPipelines.Build.Settings;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Build.Modules;

[RequiresCapability("ci-master")]
[RunIf<ModularPipelines.OnLinux>]
[ProducesArtifact("build-output", "../../_build-output.tar.gz")]
public class BuildSolutionsModule(IOptions<PipelineSettings> pipelineSettings) : Module<CommandResult[]>
{
    // Hosted single-node compilation takes about 29 minutes before the remaining solutions and archive.
    private static readonly TimeSpan BuildTimeout = TimeSpan.FromMinutes(60);

    protected override void Configure(ModuleConfigurationBuilder module) => module
        .WithTimeout(BuildTimeout);

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
                    // The combined generated integrations exhaust the shared compiler's memory in CI.
                    // Compile one project at a time; each compiler exits after its project.
                    Arguments = solution == "ModularPipelines.All.slnx"
                        ? ["/m:1", "-p:UseSharedCompilation=false"]
                        : [],
                }, new CommandExecutionOptions
                {
                    ExecutionTimeout = BuildTimeout,
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
