using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "target-grpc-proxies", "delete")]
public record GcloudComputeTargetGrpcProxiesDeleteOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;