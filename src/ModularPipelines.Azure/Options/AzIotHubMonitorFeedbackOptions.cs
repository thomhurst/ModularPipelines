using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "hub", "monitor-feedback")]
public record AzIotHubMonitorFeedbackOptions : AzOptions
{
    [CliOption("--auth-type")]
    public string? AuthType { get; set; }

    [CliOption("--device-id")]
    public string? DeviceId { get; set; }

    [CliOption("--hub-name")]
    public string? HubName { get; set; }

    [CliOption("--login")]
    public string? Login { get; set; }

    [CliFlag("--repair")]
    public bool? Repair { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--wait-on-msg")]
    public string? WaitOnMsg { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}