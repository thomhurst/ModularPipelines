using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "private-cloud", "create")]
public record AzVmwarePrivateCloudCreateOptions(
[property: CommandSwitch("--cluster-size")] string ClusterSize,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--network-block")] string NetworkBlock,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--sku")] string Sku
) : AzOptions
{
    [BooleanCommandSwitch("--accept-eula")]
    public bool? AcceptEula { get; set; }

    [CommandSwitch("--ext-nw-blocks")]
    public string? ExtNwBlocks { get; set; }

    [CommandSwitch("--internet")]
    public string? Internet { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--mi-system-assigned")]
    public bool? MiSystemAssigned { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--nsxt-password")]
    public string? NsxtPassword { get; set; }

    [CommandSwitch("--secondary-zone")]
    public string? SecondaryZone { get; set; }

    [CommandSwitch("--strategy")]
    public string? Strategy { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--vcenter-password")]
    public string? VcenterPassword { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}

