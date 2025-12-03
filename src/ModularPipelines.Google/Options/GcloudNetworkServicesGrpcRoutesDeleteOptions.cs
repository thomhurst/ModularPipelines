using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-services", "grpc-routes", "delete")]
public record GcloudNetworkServicesGrpcRoutesDeleteOptions(
[property: CliArgument] string GrpcRoute,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}