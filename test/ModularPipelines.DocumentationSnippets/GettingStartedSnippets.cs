using ModularPipelines.Context;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.DocumentationSnippets;

public static class GettingStartedSnippets
{
    public static async Task ConfigurePipeline(string[] args)
    {
        var builder = Pipeline.CreateBuilder(args);
        builder.AddModule<BuildModule>();

        await builder.RunAsync();
    }

    public sealed class BuildModule : Module<CommandResult>
    {
        protected override async Task<CommandResult> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return await context.Tools.DotNet.BuildAsync(
                new DotNetBuildOptions
                {
                    ProjectSolution = "../MySolution.slnx",
                    Configuration = "Release",
                },
                cancellationToken: cancellationToken);
        }
    }
}
