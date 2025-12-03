using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "private-clouds", "management-dns-zone-bindings", "update")]
public record GcloudVmwarePrivateCloudsManagementDnsZoneBindingsUpdateOptions(
[property: CliArgument] string ManagementDnsZoneBinding,
[property: CliArgument] string Location,
[property: CliArgument] string PrivateCloud,
[property: CliOption("--description")] string Description
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}