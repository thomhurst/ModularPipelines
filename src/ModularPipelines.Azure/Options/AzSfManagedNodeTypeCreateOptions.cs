using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sf", "managed-node-type", "create")]
public record AzSfManagedNodeTypeCreateOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--instance-count")] int InstanceCount,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--app-end-port")]
    public string? AppEndPort { get; set; }

    [CliOption("--app-start-port")]
    public string? AppStartPort { get; set; }

    [CliOption("--capacity")]
    public string? Capacity { get; set; }

    [CliOption("--data-disk-size")]
    public string? DataDiskSize { get; set; }

    [CliOption("--data-disk-type")]
    public string? DataDiskType { get; set; }

    [CliOption("--ephemeral-end-port")]
    public string? EphemeralEndPort { get; set; }

    [CliOption("--ephemeral-start-port")]
    public string? EphemeralStartPort { get; set; }

    [CliFlag("--is-stateless")]
    public bool? IsStateless { get; set; }

    [CliFlag("--multi-place-groups")]
    public bool? MultiPlaceGroups { get; set; }

    [CliOption("--placement-property")]
    public string? PlacementProperty { get; set; }

    [CliFlag("--primary")]
    public bool? Primary { get; set; }

    [CliOption("--vm-image-offer")]
    public string? VmImageOffer { get; set; }

    [CliOption("--vm-image-publisher")]
    public string? VmImagePublisher { get; set; }

    [CliOption("--vm-image-sku")]
    public string? VmImageSku { get; set; }

    [CliOption("--vm-image-version")]
    public string? VmImageVersion { get; set; }

    [CliOption("--vm-size")]
    public string? VmSize { get; set; }
}