using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "device", "simulate")]
public record AzIotDeviceSimulateOptions(
[property: CliOption("--device-id")] string DeviceId
) : AzOptions
{
    [CliOption("--certificate-file-path")]
    public string? CertificateFilePath { get; set; }

    [CliOption("--da")]
    public string? Da { get; set; }

    [CliOption("--dtmi")]
    public string? Dtmi { get; set; }

    [CliOption("--hub-name")]
    public string? HubName { get; set; }

    [CliOption("--init-reported-properties")]
    public string? InitReportedProperties { get; set; }

    [CliOption("--key")]
    public string? Key { get; set; }

    [CliOption("--key-file-path")]
    public string? KeyFilePath { get; set; }

    [CliOption("--login")]
    public string? Login { get; set; }

    [CliOption("--mc")]
    public string? Mc { get; set; }

    [CliOption("--method-response-code")]
    public string? MethodResponseCode { get; set; }

    [CliOption("--method-response-payload")]
    public string? MethodResponsePayload { get; set; }

    [CliOption("--mi")]
    public string? Mi { get; set; }

    [CliOption("--pass")]
    public string? Pass { get; set; }

    [CliOption("--properties")]
    public string? Properties { get; set; }

    [CliOption("--proto")]
    public string? Proto { get; set; }

    [CliOption("--receive-settle")]
    public string? ReceiveSettle { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}