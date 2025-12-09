using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vmware", "private-cloud", "create")]
public record AzVmwarePrivateCloudCreateOptions(
[property: CliOption("--cluster-size")] string ClusterSize,
[property: CliOption("--name")] string Name,
[property: CliOption("--network-block")] string NetworkBlock,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--sku")] string Sku
) : AzOptions
{
    [CliFlag("--accept-eula")]
    public bool? AcceptEula { get; set; }

    [CliOption("--ext-nw-blocks")]
    public string? ExtNwBlocks { get; set; }

    [CliOption("--internet")]
    public string? Internet { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--mi-system-assigned")]
    public bool? MiSystemAssigned { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--nsxt-password")]
    public string? NsxtPassword { get; set; }

    [CliOption("--secondary-zone")]
    public string? SecondaryZone { get; set; }

    [CliOption("--strategy")]
    public string? Strategy { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--vcenter-password")]
    public string? VcenterPassword { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}