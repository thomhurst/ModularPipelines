using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "private-clouds", "external-addresses", "describe")]
public record GcloudVmwarePrivateCloudsExternalAddressesDescribeOptions(
[property: CliArgument] string ExternalAddress,
[property: CliArgument] string Location,
[property: CliArgument] string PrivateCloud
) : GcloudOptions;