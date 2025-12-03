using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "virtual-appliance", "create")]
public record AzNetworkVirtualApplianceCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
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

    [CliOption("--identity")]
    public string? Identity { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--scale-unit")]
    public string? ScaleUnit { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--vendor")]
    public string? Vendor { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }

    [CliOption("--vhub")]
    public string? Vhub { get; set; }
}