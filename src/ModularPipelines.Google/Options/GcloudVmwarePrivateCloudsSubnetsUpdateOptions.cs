using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "private-clouds", "subnets", "update")]
public record GcloudVmwarePrivateCloudsSubnetsUpdateOptions(
[property: PositionalArgument] string Subnet,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string PrivateCloud,
[property: CommandSwitch("--ip-cidr-range")] string IpCidrRange
) : GcloudOptions;