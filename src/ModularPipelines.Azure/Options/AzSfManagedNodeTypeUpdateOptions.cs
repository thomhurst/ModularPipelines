using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sf", "managed-node-type", "update")]
public record AzSfManagedNodeTypeUpdateOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--app-end-port")]
    public string? AppEndPort { get; set; }

    [CommandSwitch("--app-start-port")]
    public string? AppStartPort { get; set; }

    [CommandSwitch("--capacity")]
    public string? Capacity { get; set; }

    [CommandSwitch("--ephemeral-end-port")]
    public string? EphemeralEndPort { get; set; }

    [CommandSwitch("--ephemeral-start-port")]
    public string? EphemeralStartPort { get; set; }

    [CommandSwitch("--instance-count")]
    public int? InstanceCount { get; set; }

    [CommandSwitch("--placement-property")]
    public string? PlacementProperty { get; set; }
}