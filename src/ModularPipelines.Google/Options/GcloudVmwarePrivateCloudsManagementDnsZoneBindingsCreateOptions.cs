using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "private-clouds", "management-dns-zone-bindings", "create")]
public record GcloudVmwarePrivateCloudsManagementDnsZoneBindingsCreateOptions(
[property: PositionalArgument] string ManagementDnsZoneBinding,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string PrivateCloud,
[property: CommandSwitch("--vmware-engine-network")] string VmwareEngineNetwork,
[property: CommandSwitch("--vpc-network")] string VpcNetwork
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }
}