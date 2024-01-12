using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-services", "grpc-routes", "import")]
public record GcloudNetworkServicesGrpcRoutesImportOptions(
[property: PositionalArgument] string GrpcRoute,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }
}