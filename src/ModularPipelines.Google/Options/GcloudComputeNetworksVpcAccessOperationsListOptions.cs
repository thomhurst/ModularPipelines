using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "networks", "vpc-access", "operations", "list")]
public record GcloudComputeNetworksVpcAccessOperationsListOptions(
[property: CliOption("--region")] string Region
) : GcloudOptions;