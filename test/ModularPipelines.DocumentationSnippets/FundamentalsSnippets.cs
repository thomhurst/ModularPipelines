using ModularPipelines;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Options;

namespace ModularPipelines.DocumentationSnippets;

public static class FundamentalsSnippets
{
    public static async Task Build(IModuleContext context, CancellationToken cancellationToken)
    {
        await context.Tools.DotNet.BuildAsync(new DotNetBuildOptions
        {
            ProjectSolution = "MyApp.sln"
        }, cancellationToken: cancellationToken);
    }
}
