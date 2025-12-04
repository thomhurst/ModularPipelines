using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "device", "upload-file")]
public record AzIotDeviceUploadFileOptions(
[property: CliOption("--content-type")] string ContentType,
[property: CliOption("--device-id")] string DeviceId,
[property: CliOption("--file-path")] string FilePath
) : AzOptions
{
    [CliOption("--hub-name")]
    public string? HubName { get; set; }

    [CliOption("--login")]
    public string? Login { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}