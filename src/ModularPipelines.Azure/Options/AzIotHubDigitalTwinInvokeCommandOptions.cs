using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "hub", "digital-twin", "invoke-command")]
public record AzIotHubDigitalTwinInvokeCommandOptions(
[property: CliOption("--cn")] string Cn,
[property: CliOption("--device-id")] string DeviceId
) : AzOptions
{
    [CliOption("--component-path")]
    public string? ComponentPath { get; set; }

    [CliOption("--connect-timeout")]
    public string? ConnectTimeout { get; set; }

    [CliOption("--hub-name")]
    public string? HubName { get; set; }

    [CliOption("--login")]
    public string? Login { get; set; }

    [CliOption("--payload")]
    public string? Payload { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--response-timeout")]
    public string? ResponseTimeout { get; set; }
}