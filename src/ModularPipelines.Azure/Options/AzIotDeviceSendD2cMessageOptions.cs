using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "device", "send-d2c-message")]
public record AzIotDeviceSendD2cMessageOptions(
[property: CliOption("--device-id")] string DeviceId
) : AzOptions
{
    [CliOption("--certificate-file-path")]
    public string? CertificateFilePath { get; set; }

    [CliOption("--da")]
    public string? Da { get; set; }

    [CliOption("--data-file-path")]
    public string? DataFilePath { get; set; }

    [CliOption("--dtmi")]
    public string? Dtmi { get; set; }

    [CliOption("--hub-name")]
    public string? HubName { get; set; }

    [CliOption("--key")]
    public string? Key { get; set; }

    [CliOption("--key-file-path")]
    public string? KeyFilePath { get; set; }

    [CliOption("--login")]
    public string? Login { get; set; }

    [CliOption("--mc")]
    public string? Mc { get; set; }

    [CliOption("--pass")]
    public string? Pass { get; set; }

    [CliOption("--properties")]
    public string? Properties { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}