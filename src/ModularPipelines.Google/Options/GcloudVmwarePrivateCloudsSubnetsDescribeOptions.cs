using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "private-clouds", "subnets", "describe")]
public record GcloudVmwarePrivateCloudsSubnetsDescribeOptions(
[property: CliArgument] string Subnet,
[property: CliArgument] string Location,
[property: CliArgument] string PrivateCloud
) : GcloudOptions;