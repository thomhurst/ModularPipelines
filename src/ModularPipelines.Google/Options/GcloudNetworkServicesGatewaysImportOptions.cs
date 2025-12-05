using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-services", "gateways", "import")]
public record GcloudNetworkServicesGatewaysImportOptions(
[property: CliArgument] string Gateway,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }
}