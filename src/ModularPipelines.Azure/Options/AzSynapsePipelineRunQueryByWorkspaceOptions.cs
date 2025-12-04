using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse", "pipeline-run", "query-by-workspace")]
public record AzSynapsePipelineRunQueryByWorkspaceOptions(
[property: CliOption("--last-updated-after")] string LastUpdatedAfter,
[property: CliOption("--last-updated-before")] string LastUpdatedBefore,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--continuation-token")]
    public string? ContinuationToken { get; set; }

    [CliOption("--filters")]
    public string? Filters { get; set; }

    [CliOption("--order-by")]
    public string? OrderBy { get; set; }
}