using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("signalr", "replica", "show")]
public record AzSignalrReplicaShowOptions : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--replica-name")]
    public string? ReplicaName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--signalr-name")]
    public string? SignalrName { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}