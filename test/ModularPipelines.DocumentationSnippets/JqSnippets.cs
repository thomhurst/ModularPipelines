using ModularPipelines;
using ModularPipelines.Context;
using ModularPipelines.Jq.Options;

#pragma warning disable IDE0130 // Use a consumer namespace to verify the documented imports.
namespace DocumentationExamples;

// docs/docs/mp-packages/jq.md
public static class JqSnippets
{
    // Qualify the harness context so CliValuePair verifies the root import independently.
    public static async Task ProcessJson(ModularPipelines.IModuleContext context, CancellationToken cancellationToken)
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
