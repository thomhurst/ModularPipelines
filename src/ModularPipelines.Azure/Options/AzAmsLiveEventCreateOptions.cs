using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ams", "live-event", "create")]
public record AzAmsLiveEventCreateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--ips")] string Ips,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--streaming-protocol")] string StreamingProtocol
) : AzOptions
{
    [CliOption("--access-token")]
    public string? AccessToken { get; set; }

    [CliOption("--alternative-media-id")]
    public string? AlternativeMediaId { get; set; }

    [CliFlag("--auto-start")]
    public bool? AutoStart { get; set; }

    [CliOption("--client-access-policy")]
    public string? ClientAccessPolicy { get; set; }

    [CliOption("--cross-domain-policy")]
    public string? CrossDomainPolicy { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--encoding-type")]
    public string? EncodingType { get; set; }

    [CliOption("--hostname-prefix")]
    public string? HostnamePrefix { get; set; }

    [CliOption("--key-frame-interval")]
    public string? KeyFrameInterval { get; set; }

    [CliOption("--key-frame-interval-duration")]
    public string? KeyFrameIntervalDuration { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--preset-name")]
    public string? PresetName { get; set; }

    [CliOption("--preview-ips")]
    public string? PreviewIps { get; set; }

    [CliOption("--preview-locator")]
    public string? PreviewLocator { get; set; }

    [CliOption("--stream-options")]
    public string? StreamOptions { get; set; }

    [CliOption("--streaming-policy-name")]
    public string? StreamingPolicyName { get; set; }

    [CliOption("--stretch-mode")]
    public string? StretchMode { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--transcription-lang")]
    public string? TranscriptionLang { get; set; }

    [CliFlag("--use-static-hostname")]
    public bool? UseStaticHostname { get; set; }
}