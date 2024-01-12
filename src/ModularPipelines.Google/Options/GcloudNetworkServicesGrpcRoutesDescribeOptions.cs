using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-services", "grpc-routes", "describe")]
public record GcloudNetworkServicesGrpcRoutesDescribeOptions(
[property: PositionalArgument] string GrpcRoute,
[property: PositionalArgument] string Location
) : GcloudOptions;