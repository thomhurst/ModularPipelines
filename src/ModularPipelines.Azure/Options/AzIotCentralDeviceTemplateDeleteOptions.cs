using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "central", "device-template", "delete")]
public record AzIotCentralDeviceTemplateDeleteOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--device-template-id")] string DeviceTemplateId
) : AzOptions
{
    [CliOption("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}