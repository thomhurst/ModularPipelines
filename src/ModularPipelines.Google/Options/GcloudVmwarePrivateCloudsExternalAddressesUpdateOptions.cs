using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "private-clouds", "external-addresses", "update")]
public record GcloudVmwarePrivateCloudsExternalAddressesUpdateOptions(
[property: PositionalArgument] string ExternalAddress,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string PrivateCloud
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--internal-ip")]
    public string? InternalIp { get; set; }
}