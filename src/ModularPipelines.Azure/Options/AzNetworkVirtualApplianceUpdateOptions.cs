using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "virtual-appliance", "update")]
public record AzNetworkVirtualApplianceUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

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

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--identity")]
    public string? Identity { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--scale-unit")]
    public string? ScaleUnit { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--vendor")]
    public string? Vendor { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }

    [CommandSwitch("--vhub")]
    public string? Vhub { get; set; }
}