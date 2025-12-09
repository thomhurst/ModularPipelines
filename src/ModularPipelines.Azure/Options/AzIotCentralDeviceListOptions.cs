using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "central", "device", "list")]
public record AzIotCentralDeviceListOptions(
[property: CliOption("--app-id")] string AppId
) : AzOptions
{
    [CliOption("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CliFlag("--edge-only")]
    public bool? EdgeOnly { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}