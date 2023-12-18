using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("signalr", "replica", "create")]
public record AzSignalrReplicaCreateOptions(
[property: CommandSwitch("--replica-name")] string ReplicaName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--signalr-name")] string SignalrName,
[property: CommandSwitch("--sku")] string Sku
) : AzOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--unit-count")]
    public int? UnitCount { get; set; }
}

