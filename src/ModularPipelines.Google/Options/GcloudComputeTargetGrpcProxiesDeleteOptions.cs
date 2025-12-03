using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "target-grpc-proxies", "delete")]
public record GcloudComputeTargetGrpcProxiesDeleteOptions(
[property: CliArgument] string Name
) : GcloudOptions;