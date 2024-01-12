using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "private-clouds", "management-dns-zone-bindings", "delete")]
public record GcloudVmwarePrivateCloudsManagementDnsZoneBindingsDeleteOptions(
[property: PositionalArgument] string ManagementDnsZoneBinding,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string PrivateCloud
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}