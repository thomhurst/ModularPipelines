using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ams", "streaming-policy", "create")]
public record AzAmsStreamingPolicyCreateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--cbcs-clear-tracks")]
    public string? CbcsClearTracks { get; set; }

    [CliOption("--cbcs-default-key-label")]
    public string? CbcsDefaultKeyLabel { get; set; }

    [CliOption("--cbcs-default-key-policy-name")]
    public string? CbcsDefaultKeyPolicyName { get; set; }

    [CliFlag("--cbcs-fair-play-allow-persistent-license")]
    public bool? CbcsFairPlayAllowPersistentLicense { get; set; }

    [CliOption("--cbcs-fair-play-template")]
    public string? CbcsFairPlayTemplate { get; set; }

    [CliOption("--cbcs-key-to-track-mappings")]
    public string? CbcsKeyToTrackMappings { get; set; }

    [CliOption("--cbcs-protocols")]
    public string? CbcsProtocols { get; set; }

    [CliOption("--cenc-clear-tracks")]
    public string? CencClearTracks { get; set; }

    [CliOption("--cenc-default-key-label")]
    public string? CencDefaultKeyLabel { get; set; }

    [CliOption("--cenc-default-key-policy-name")]
    public string? CencDefaultKeyPolicyName { get; set; }

    [CliFlag("--cenc-disable-play-ready")]
    public bool? CencDisablePlayReady { get; set; }

    [CliFlag("--cenc-disable-widevine")]
    public bool? CencDisableWidevine { get; set; }

    [CliOption("--cenc-key-to-track-mappings")]
    public string? CencKeyToTrackMappings { get; set; }

    [CliOption("--cenc-play-ready-attributes")]
    public string? CencPlayReadyAttributes { get; set; }

    [CliOption("--cenc-play-ready-template")]
    public string? CencPlayReadyTemplate { get; set; }

    [CliOption("--cenc-protocols")]
    public string? CencProtocols { get; set; }

    [CliOption("--cenc-widevine-template")]
    public string? CencWidevineTemplate { get; set; }

    [CliOption("--default-content-key-policy-name")]
    public string? DefaultContentKeyPolicyName { get; set; }

    [CliOption("--envelope-clear-tracks")]
    public string? EnvelopeClearTracks { get; set; }

    [CliOption("--envelope-default-key-label")]
    public string? EnvelopeDefaultKeyLabel { get; set; }

    [CliOption("--envelope-default-key-policy-name")]
    public string? EnvelopeDefaultKeyPolicyName { get; set; }

    [CliOption("--envelope-key-to-track-mappings")]
    public string? EnvelopeKeyToTrackMappings { get; set; }

    [CliOption("--envelope-protocols")]
    public string? EnvelopeProtocols { get; set; }

    [CliOption("--envelope-template")]
    public string? EnvelopeTemplate { get; set; }

    [CliFlag("--no-encryption-protocols")]
    public bool? NoEncryptionProtocols { get; set; }
}