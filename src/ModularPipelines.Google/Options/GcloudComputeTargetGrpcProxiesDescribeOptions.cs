using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "target-grpc-proxies", "describe")]
public record GcloudComputeTargetGrpcProxiesDescribeOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;