using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-services", "grpc-routes", "describe")]
public record GcloudNetworkServicesGrpcRoutesDescribeOptions(
[property: CliArgument] string GrpcRoute,
[property: CliArgument] string Location
) : GcloudOptions;