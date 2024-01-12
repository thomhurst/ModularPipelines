using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-services", "grpc-routes", "list")]
public record GcloudNetworkServicesGrpcRoutesListOptions(
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;