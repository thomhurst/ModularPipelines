using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "device", "upload-file")]
public record AzIotDeviceUploadFileOptions(
[property: CommandSwitch("--content-type")] string ContentType,
[property: CommandSwitch("--device-id")] string DeviceId,
[property: CommandSwitch("--file-path")] string FilePath
) : AzOptions
{
    [CommandSwitch("--hub-name")]
    public string? HubName { get; set; }

    [CommandSwitch("--login")]
    public string? Login { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}