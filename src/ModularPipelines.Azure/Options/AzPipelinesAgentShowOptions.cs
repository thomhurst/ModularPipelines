using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pipelines", "agent", "show")]
public record AzPipelinesAgentShowOptions(
[property: CommandSwitch("--agent-id")] string AgentId,
[property: CommandSwitch("--pool-id")] string PoolId
) : AzOptions
{
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