using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("services", "vpc-peerings", "operations", "describe")]
public record GcloudServicesVpcPeeringsOperationsDescribeOptions(
[property: CliOption("--name")] string Name
) : GcloudOptions;