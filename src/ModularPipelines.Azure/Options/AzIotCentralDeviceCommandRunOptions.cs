using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "central", "device", "command", "run")]
public record AzIotCentralDeviceCommandRunOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--cn")] string Cn,
[property: CliOption("--content")] string Content,
[property: CliOption("--device-id")] string DeviceId
) : AzOptions
{
    [CliOption("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CliOption("--co")]
    public string? Co { get; set; }

    [CliOption("--interface-id")]
    public string? InterfaceId { get; set; }

    [CliOption("--mn")]
    public string? Mn { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}