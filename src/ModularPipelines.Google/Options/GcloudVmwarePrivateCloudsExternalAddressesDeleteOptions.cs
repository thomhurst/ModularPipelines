using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "private-clouds", "external-addresses", "delete")]
public record GcloudVmwarePrivateCloudsExternalAddressesDeleteOptions(
[property: PositionalArgument] string ExternalAddress,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string PrivateCloud
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}