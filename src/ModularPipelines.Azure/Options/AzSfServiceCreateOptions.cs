using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sf", "service", "create")]
public record AzSfServiceCreateOptions(
[property: CommandSwitch("--application")] string Application,
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service-type")] string ServiceType,
[property: CommandSwitch("--state")] string State
) : AzOptions
{
    [CommandSwitch("--default-move-cost")]
    public string? DefaultMoveCost { get; set; }

    [CommandSwitch("--instance-count")]
    public int? InstanceCount { get; set; }

    [CommandSwitch("--min-replica")]
    public string? MinReplica { get; set; }

    [CommandSwitch("--partition-scheme")]
    public string? PartitionScheme { get; set; }

    [CommandSwitch("--target-replica")]
    public string? TargetReplica { get; set; }
}

