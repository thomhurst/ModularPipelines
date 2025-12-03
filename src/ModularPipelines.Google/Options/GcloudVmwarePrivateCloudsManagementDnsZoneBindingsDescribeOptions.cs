using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "private-clouds", "management-dns-zone-bindings", "describe")]
public record GcloudVmwarePrivateCloudsManagementDnsZoneBindingsDescribeOptions(
[property: CliArgument] string ManagementDnsZoneBinding,
[property: CliArgument] string Location,
[property: CliArgument] string PrivateCloud
) : GcloudOptions;