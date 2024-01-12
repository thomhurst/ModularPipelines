using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "private-clouds", "external-addresses", "create")]
public record GcloudVmwarePrivateCloudsExternalAddressesCreateOptions(
[property: PositionalArgument] string ExternalAddress,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string PrivateCloud,
[property: CommandSwitch("--internal-ip")] string InternalIp
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }
}