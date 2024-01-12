using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "private-clouds", "management-dns-zone-bindings", "list")]
public record GcloudVmwarePrivateCloudsManagementDnsZoneBindingsListOptions(
[property: CommandSwitch("--private-cloud")] string PrivateCloud,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;