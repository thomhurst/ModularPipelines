using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "virtual-appliance", "update")]
public record AzNetworkVirtualApplianceUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--additional-nics")]
    public string? AdditionalNics { get; set; }

    [CliOption("--asn")]
    public string? Asn { get; set; }

    [CliOption("--boot-blobs")]
    public string? BootBlobs { get; set; }

    [CliOption("--cloud-blobs")]
    public string? CloudBlobs { get; set; }

    [CliOption("--cloud-init-config")]
    public string? CloudInitConfig { get; set; }

    [CliOption("--delegation")]
    public string? Delegation { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--identity")]
    public string? Identity { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--scale-unit")]
    public string? ScaleUnit { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--vendor")]
    public string? Vendor { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }

    [CliOption("--vhub")]
    public string? Vhub { get; set; }
}