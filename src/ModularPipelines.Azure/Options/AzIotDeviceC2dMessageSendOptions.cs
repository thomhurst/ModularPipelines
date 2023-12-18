using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "device", "c2d-message", "send")]
public record AzIotDeviceC2dMessageSendOptions(
[property: CommandSwitch("--device-id")] string DeviceId
) : AzOptions
{
    [CommandSwitch("--ack")]
    public string? Ack { get; set; }

    [CommandSwitch("--auth-type")]
    public string? AuthType { get; set; }

    [CommandSwitch("--ce")]
    public string? Ce { get; set; }

    [CommandSwitch("--cid")]
    public string? Cid { get; set; }

    [CommandSwitch("--content-type")]
    public string? ContentType { get; set; }

    [CommandSwitch("--da")]
    public string? Da { get; set; }

    [CommandSwitch("--data-file-path")]
    public string? DataFilePath { get; set; }

    [CommandSwitch("--expiry")]
    public string? Expiry { get; set; }

    [CommandSwitch("--hub-name")]
    public string? HubName { get; set; }

    [CommandSwitch("--login")]
    public string? Login { get; set; }

    [CommandSwitch("--message-id")]
    public string? MessageId { get; set; }

    [CommandSwitch("--properties")]
    public string? Properties { get; set; }

    [BooleanCommandSwitch("--repair")]
    public bool? Repair { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--uid")]
    public string? Uid { get; set; }

    [BooleanCommandSwitch("--wait")]
    public bool? Wait { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}