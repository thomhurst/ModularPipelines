using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "virtual-appliance", "create")]
public record AzNetworkVirtualApplianceCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--additional-nics")]
    public string? AdditionalNics { get; set; }

    [CommandSwitch("--asn")]
    public string? Asn { get; set; }

    [CommandSwitch("--boot-blobs")]
    public string? BootBlobs { get; set; }

    [CommandSwitch("--cloud-blobs")]
    public string? CloudBlobs { get; set; }

    [CommandSwitch("--cloud-init-config")]
    public string? CloudInitConfig { get; set; }

    [CommandSwitch("--delegation")]
    public string? Delegation { get; set; }

    [CommandSwitch("--identity")]
    public string? Identity { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--scale-unit")]
    public string? ScaleUnit { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--vendor")]
    public string? Vendor { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }

    [CommandSwitch("--vhub")]
    public string? Vhub { get; set; }
}