using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "networks", "vpc-access", "connectors", "describe")]
public record GcloudComputeNetworksVpcAccessConnectorsDescribeOptions(
[property: PositionalArgument] string Connector,
[property: PositionalArgument] string Region
) : GcloudOptions;