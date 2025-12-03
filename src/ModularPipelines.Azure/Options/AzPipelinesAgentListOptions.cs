using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pipelines", "agent", "list")]
public record AzPipelinesAgentListOptions(
[property: CliOption("--pool-id")] string PoolId
) : AzOptions
{
    [CliOption("--agent-name")]
    public string? AgentName { get; set; }

    [CliOption("--demands")]
    public string? Demands { get; set; }

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