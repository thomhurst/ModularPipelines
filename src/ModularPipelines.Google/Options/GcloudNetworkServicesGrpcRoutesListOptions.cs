using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-services", "grpc-routes", "list")]
public record GcloudNetworkServicesGrpcRoutesListOptions(
[property: CliOption("--location")] string Location
) : GcloudOptions;