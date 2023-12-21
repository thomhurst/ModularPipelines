using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage-mover", "agent", "show")]
public record AzStorageMoverAgentShowOptions : AzOptions
{
    [CommandSwitch("--agent-name")]
    public string? AgentName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--storage-mover-name")]
    public string? StorageMoverName { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}