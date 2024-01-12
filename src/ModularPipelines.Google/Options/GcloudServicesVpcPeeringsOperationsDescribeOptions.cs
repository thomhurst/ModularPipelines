using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("services", "vpc-peerings", "operations", "describe")]
public record GcloudServicesVpcPeeringsOperationsDescribeOptions(
[property: CommandSwitch("--name")] string Name
) : GcloudOptions;