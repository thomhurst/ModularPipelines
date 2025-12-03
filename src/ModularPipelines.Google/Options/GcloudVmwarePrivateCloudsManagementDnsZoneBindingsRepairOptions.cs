using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "private-clouds", "management-dns-zone-bindings", "repair")]
public record GcloudVmwarePrivateCloudsManagementDnsZoneBindingsRepairOptions(
[property: CliArgument] string ManagementDnsZoneBinding,
[property: CliArgument] string Location,
[property: CliArgument] string PrivateCloud
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}