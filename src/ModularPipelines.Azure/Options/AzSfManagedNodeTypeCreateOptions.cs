using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sf", "managed-node-type", "create")]
public record AzSfManagedNodeTypeCreateOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--instance-count")] int InstanceCount,
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

    [CommandSwitch("--data-disk-size")]
    public string? DataDiskSize { get; set; }

    [CommandSwitch("--data-disk-type")]
    public string? DataDiskType { get; set; }

    [CommandSwitch("--ephemeral-end-port")]
    public string? EphemeralEndPort { get; set; }

    [CommandSwitch("--ephemeral-start-port")]
    public string? EphemeralStartPort { get; set; }

    [BooleanCommandSwitch("--is-stateless")]
    public bool? IsStateless { get; set; }

    [BooleanCommandSwitch("--multi-place-groups")]
    public bool? MultiPlaceGroups { get; set; }

    [CommandSwitch("--placement-property")]
    public string? PlacementProperty { get; set; }

    [BooleanCommandSwitch("--primary")]
    public bool? Primary { get; set; }

    [CommandSwitch("--vm-image-offer")]
    public string? VmImageOffer { get; set; }

    [CommandSwitch("--vm-image-publisher")]
    public string? VmImagePublisher { get; set; }

    [CommandSwitch("--vm-image-sku")]
    public string? VmImageSku { get; set; }

    [CommandSwitch("--vm-image-version")]
    public string? VmImageVersion { get; set; }

    [CommandSwitch("--vm-size")]
    public string? VmSize { get; set; }
}