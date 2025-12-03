using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "central", "device-template", "show")]
public record AzIotCentralDeviceTemplateShowOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--device-template-id")] string DeviceTemplateId
) : AzOptions
{
    [CliOption("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}