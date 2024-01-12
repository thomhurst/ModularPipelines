using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-services", "tls-routes", "export")]
public record GcloudNetworkServicesTlsRoutesExportOptions(
[property: PositionalArgument] string TlsRoute,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--destination")]
    public string? Destination { get; set; }
}