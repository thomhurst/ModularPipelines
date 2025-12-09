using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse", "trigger-run", "query-by-workspace")]
public record AzSynapseTriggerRunQueryByWorkspaceOptions(
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