using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("pipelines", "agent", "show")]
public record AzPipelinesAgentShowOptions(
[property: CliOption("--agent-id")] string AgentId,
[property: CliOption("--pool-id")] string PoolId
) : AzOptions
{
    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliFlag("--include-assigned-request")]
    public bool? IncludeAssignedRequest { get; set; }

    [CliFlag("--include-capabilities")]
    public bool? IncludeCapabilities { get; set; }

    [CliFlag("--include-last-completed-request")]
    public bool? IncludeLastCompletedRequest { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }
}