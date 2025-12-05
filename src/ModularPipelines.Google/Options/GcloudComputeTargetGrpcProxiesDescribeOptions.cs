using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "target-grpc-proxies", "describe")]
public record GcloudComputeTargetGrpcProxiesDescribeOptions(
[property: CliArgument] string Name
) : GcloudOptions;