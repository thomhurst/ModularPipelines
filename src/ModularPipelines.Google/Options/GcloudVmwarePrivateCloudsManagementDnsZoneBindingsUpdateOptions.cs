using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "private-clouds", "management-dns-zone-bindings", "update")]
public record GcloudVmwarePrivateCloudsManagementDnsZoneBindingsUpdateOptions(
[property: PositionalArgument] string ManagementDnsZoneBinding,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string PrivateCloud,
[property: CommandSwitch("--description")] string Description
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}