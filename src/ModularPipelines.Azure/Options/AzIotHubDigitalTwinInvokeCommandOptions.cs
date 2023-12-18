using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "hub", "digital-twin", "invoke-command")]
public record AzIotHubDigitalTwinInvokeCommandOptions(
[property: CommandSwitch("--cn")] string Cn,
[property: CommandSwitch("--device-id")] string DeviceId
) : AzOptions
{
    [CommandSwitch("--component-path")]
    public string? ComponentPath { get; set; }

    [CommandSwitch("--connect-timeout")]
    public string? ConnectTimeout { get; set; }

    [CommandSwitch("--hub-name")]
    public string? HubName { get; set; }

    [CommandSwitch("--login")]
    public string? Login { get; set; }

    [CommandSwitch("--payload")]
    public string? Payload { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--response-timeout")]
    public string? ResponseTimeout { get; set; }
}