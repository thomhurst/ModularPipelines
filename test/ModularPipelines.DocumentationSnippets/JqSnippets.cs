using ModularPipelines.Context;
using ModularPipelines.Jq.Options;
using ModularPipelines.Models;

namespace ModularPipelines.DocumentationSnippets;

// docs/docs/mp-packages/jq.md
public static class JqSnippets
{
    public static async Task ProcessJson(IModuleContext context, CancellationToken cancellationToken)
    {
        var result = await context.Tools.Jq.ExecuteAsync(
            new JqExecuteOptions
            {
                RawOutput = true,
                Indent = 2,
                Arg = [new CliValuePair("environment", "production")],
                Filter = ".deployments[] | select(.environment == $environment)",
                InputFiles = ["deployments.json"],
            },
            cancellationToken: cancellationToken);
    }
}
