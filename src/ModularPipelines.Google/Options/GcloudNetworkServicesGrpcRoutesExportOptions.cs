using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-services", "grpc-routes", "export")]
public record GcloudNetworkServicesGrpcRoutesExportOptions(
[property: CliArgument] string GrpcRoute,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--destination")]
    public string? Destination { get; set; }
}