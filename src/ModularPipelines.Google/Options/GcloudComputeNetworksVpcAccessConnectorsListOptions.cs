using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "networks", "vpc-access", "connectors", "list")]
public record GcloudComputeNetworksVpcAccessConnectorsListOptions(
[property: CliOption("--region")] string Region
) : GcloudOptions;