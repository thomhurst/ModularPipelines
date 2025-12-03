using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "networks", "vpc-access", "connectors", "describe")]
public record GcloudComputeNetworksVpcAccessConnectorsDescribeOptions(
[property: CliArgument] string Connector,
[property: CliArgument] string Region
) : GcloudOptions;