using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ams", "live-event", "create")]
public record AzAmsLiveEventCreateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--ips")] string Ips,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--streaming-protocol")] string StreamingProtocol
) : AzOptions
{
    [CommandSwitch("--access-token")]
    public string? AccessToken { get; set; }

    [CommandSwitch("--alternative-media-id")]
    public string? AlternativeMediaId { get; set; }

    [BooleanCommandSwitch("--auto-start")]
    public bool? AutoStart { get; set; }

    [CommandSwitch("--client-access-policy")]
    public string? ClientAccessPolicy { get; set; }

    [CommandSwitch("--cross-domain-policy")]
    public string? CrossDomainPolicy { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--encoding-type")]
    public string? EncodingType { get; set; }

    [CommandSwitch("--hostname-prefix")]
    public string? HostnamePrefix { get; set; }

    [CommandSwitch("--key-frame-interval")]
    public string? KeyFrameInterval { get; set; }

    [CommandSwitch("--key-frame-interval-duration")]
    public string? KeyFrameIntervalDuration { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--preset-name")]
    public string? PresetName { get; set; }

    [CommandSwitch("--preview-ips")]
    public string? PreviewIps { get; set; }

    [CommandSwitch("--preview-locator")]
    public string? PreviewLocator { get; set; }

    [CommandSwitch("--stream-options")]
    public string? StreamOptions { get; set; }

    [CommandSwitch("--streaming-policy-name")]
    public string? StreamingPolicyName { get; set; }

    [CommandSwitch("--stretch-mode")]
    public string? StretchMode { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--transcription-lang")]
    public string? TranscriptionLang { get; set; }

    [BooleanCommandSwitch("--use-static-hostname")]
    public bool? UseStaticHostname { get; set; }
}