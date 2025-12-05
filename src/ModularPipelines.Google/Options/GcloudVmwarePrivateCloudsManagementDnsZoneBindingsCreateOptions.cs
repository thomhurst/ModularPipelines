using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "private-clouds", "management-dns-zone-bindings", "create")]
public record GcloudVmwarePrivateCloudsManagementDnsZoneBindingsCreateOptions(
[property: CliArgument] string ManagementDnsZoneBinding,
[property: CliArgument] string Location,
[property: CliArgument] string PrivateCloud,
[property: CliOption("--vmware-engine-network")] string VmwareEngineNetwork,
[property: CliOption("--vpc-network")] string VpcNetwork
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }
}