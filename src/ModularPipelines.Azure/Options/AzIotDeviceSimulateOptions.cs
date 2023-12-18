using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "device", "simulate")]
public record AzIotDeviceSimulateOptions(
[property: CommandSwitch("--device-id")] string DeviceId
) : AzOptions
{
    [CommandSwitch("--certificate-file-path")]
    public string? CertificateFilePath { get; set; }

    [CommandSwitch("--da")]
    public string? Da { get; set; }

    [CommandSwitch("--dtmi")]
    public string? Dtmi { get; set; }

    [CommandSwitch("--hub-name")]
    public string? HubName { get; set; }

    [CommandSwitch("--init-reported-properties")]
    public string? InitReportedProperties { get; set; }

    [CommandSwitch("--key")]
    public string? Key { get; set; }

    [CommandSwitch("--key-file-path")]
    public string? KeyFilePath { get; set; }

    [CommandSwitch("--login")]
    public string? Login { get; set; }

    [CommandSwitch("--mc")]
    public string? Mc { get; set; }

    [CommandSwitch("--method-response-code")]
    public string? MethodResponseCode { get; set; }

    [CommandSwitch("--method-response-payload")]
    public string? MethodResponsePayload { get; set; }

    [CommandSwitch("--mi")]
    public string? Mi { get; set; }

    [CommandSwitch("--pass")]
    public string? Pass { get; set; }

    [CommandSwitch("--properties")]
    public string? Properties { get; set; }

    [CommandSwitch("--proto")]
    public string? Proto { get; set; }

    [CommandSwitch("--receive-settle")]
    public string? ReceiveSettle { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}