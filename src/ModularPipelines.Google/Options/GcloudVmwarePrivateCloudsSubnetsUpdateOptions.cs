using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "private-clouds", "subnets", "update")]
public record GcloudVmwarePrivateCloudsSubnetsUpdateOptions(
[property: CliArgument] string Subnet,
[property: CliArgument] string Location,
[property: CliArgument] string PrivateCloud,
[property: CliOption("--ip-cidr-range")] string IpCidrRange
) : GcloudOptions;