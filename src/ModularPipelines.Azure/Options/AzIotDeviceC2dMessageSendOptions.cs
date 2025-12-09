using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "device", "c2d-message", "send")]
public record AzIotDeviceC2dMessageSendOptions(
[property: CliOption("--device-id")] string DeviceId
) : AzOptions
{
    [CliOption("--ack")]
    public string? Ack { get; set; }

    [CliOption("--auth-type")]
    public string? AuthType { get; set; }

    [CliOption("--ce")]
    public string? Ce { get; set; }

    [CliOption("--cid")]
    public string? Cid { get; set; }

    [CliOption("--content-type")]
    public string? ContentType { get; set; }

    [CliOption("--da")]
    public string? Da { get; set; }

    [CliOption("--data-file-path")]
    public string? DataFilePath { get; set; }

    [CliOption("--expiry")]
    public string? Expiry { get; set; }

    [CliOption("--hub-name")]
    public string? HubName { get; set; }

    [CliOption("--login")]
    public string? Login { get; set; }

    [CliOption("--message-id")]
    public string? MessageId { get; set; }

    [CliOption("--properties")]
    public string? Properties { get; set; }

    [CliFlag("--repair")]
    public bool? Repair { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--uid")]
    public string? Uid { get; set; }

    [CliFlag("--wait")]
    public bool? Wait { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}