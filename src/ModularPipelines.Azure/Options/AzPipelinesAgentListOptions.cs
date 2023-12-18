using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pipelines", "agent", "list")]
public record AzPipelinesAgentListOptions(
[property: CommandSwitch("--pool-id")] string PoolId
) : AzOptions
{
    [CommandSwitch("--agent-name")]
    public string? AgentName { get; set; }

    [CommandSwitch("--demands")]
    public string? Demands { get; set; }

    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [BooleanCommandSwitch("--include-assigned-request")]
    public bool? IncludeAssignedRequest { get; set; }

    [BooleanCommandSwitch("--include-capabilities")]
    public bool? IncludeCapabilities { get; set; }

    [BooleanCommandSwitch("--include-last-completed-request")]
    public bool? IncludeLastCompletedRequest { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }
}