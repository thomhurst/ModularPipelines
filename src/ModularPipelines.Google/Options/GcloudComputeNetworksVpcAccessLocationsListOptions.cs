using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "networks", "vpc-access", "locations", "list")]
public record GcloudComputeNetworksVpcAccessLocationsListOptions : GcloudOptions;