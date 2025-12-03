using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "central", "device", "twin", "replace")]
public record AzIotCentralDeviceTwinReplaceOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--content")] string Content,
[property: CliOption("--device-id")] string DeviceId
) : AzOptions
{
    [CliOption("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CliOption("--co")]
    public string? Co { get; set; }

    [CliOption("--mn")]
    public string? Mn { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}