using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "device", "send-d2c-message")]
public record AzIotDeviceSendD2cMessageOptions(
[property: CommandSwitch("--device-id")] string DeviceId
) : AzOptions
{
    [CommandSwitch("--certificate-file-path")]
    public string? CertificateFilePath { get; set; }

    [CommandSwitch("--da")]
    public string? Da { get; set; }

    [CommandSwitch("--data-file-path")]
    public string? DataFilePath { get; set; }

    [CommandSwitch("--dtmi")]
    public string? Dtmi { get; set; }

    [CommandSwitch("--hub-name")]
    public string? HubName { get; set; }

    [CommandSwitch("--key")]
    public string? Key { get; set; }

    [CommandSwitch("--key-file-path")]
    public string? KeyFilePath { get; set; }

    [CommandSwitch("--login")]
    public string? Login { get; set; }

    [CommandSwitch("--mc")]
    public string? Mc { get; set; }

    [CommandSwitch("--pass")]
    public string? Pass { get; set; }

    [CommandSwitch("--properties")]
    public string? Properties { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}

