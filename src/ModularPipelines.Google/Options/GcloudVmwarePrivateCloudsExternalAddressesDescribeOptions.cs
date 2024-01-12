using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "private-clouds", "external-addresses", "describe")]
public record GcloudVmwarePrivateCloudsExternalAddressesDescribeOptions(
[property: PositionalArgument] string ExternalAddress,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string PrivateCloud
) : GcloudOptions;