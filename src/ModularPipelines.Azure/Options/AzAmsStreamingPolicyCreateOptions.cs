using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ams", "streaming-policy", "create")]
public record AzAmsStreamingPolicyCreateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--cbcs-clear-tracks")]
    public string? CbcsClearTracks { get; set; }

    [CommandSwitch("--cbcs-default-key-label")]
    public string? CbcsDefaultKeyLabel { get; set; }

    [CommandSwitch("--cbcs-default-key-policy-name")]
    public string? CbcsDefaultKeyPolicyName { get; set; }

    [BooleanCommandSwitch("--cbcs-fair-play-allow-persistent-license")]
    public bool? CbcsFairPlayAllowPersistentLicense { get; set; }

    [CommandSwitch("--cbcs-fair-play-template")]
    public string? CbcsFairPlayTemplate { get; set; }

    [CommandSwitch("--cbcs-key-to-track-mappings")]
    public string? CbcsKeyToTrackMappings { get; set; }

    [CommandSwitch("--cbcs-protocols")]
    public string? CbcsProtocols { get; set; }

    [CommandSwitch("--cenc-clear-tracks")]
    public string? CencClearTracks { get; set; }

    [CommandSwitch("--cenc-default-key-label")]
    public string? CencDefaultKeyLabel { get; set; }

    [CommandSwitch("--cenc-default-key-policy-name")]
    public string? CencDefaultKeyPolicyName { get; set; }

    [BooleanCommandSwitch("--cenc-disable-play-ready")]
    public bool? CencDisablePlayReady { get; set; }

    [BooleanCommandSwitch("--cenc-disable-widevine")]
    public bool? CencDisableWidevine { get; set; }

    [CommandSwitch("--cenc-key-to-track-mappings")]
    public string? CencKeyToTrackMappings { get; set; }

    [CommandSwitch("--cenc-play-ready-attributes")]
    public string? CencPlayReadyAttributes { get; set; }

    [CommandSwitch("--cenc-play-ready-template")]
    public string? CencPlayReadyTemplate { get; set; }

    [CommandSwitch("--cenc-protocols")]
    public string? CencProtocols { get; set; }

    [CommandSwitch("--cenc-widevine-template")]
    public string? CencWidevineTemplate { get; set; }

    [CommandSwitch("--default-content-key-policy-name")]
    public string? DefaultContentKeyPolicyName { get; set; }

    [CommandSwitch("--envelope-clear-tracks")]
    public string? EnvelopeClearTracks { get; set; }

    [CommandSwitch("--envelope-default-key-label")]
    public string? EnvelopeDefaultKeyLabel { get; set; }

    [CommandSwitch("--envelope-default-key-policy-name")]
    public string? EnvelopeDefaultKeyPolicyName { get; set; }

    [CommandSwitch("--envelope-key-to-track-mappings")]
    public string? EnvelopeKeyToTrackMappings { get; set; }

    [CommandSwitch("--envelope-protocols")]
    public string? EnvelopeProtocols { get; set; }

    [CommandSwitch("--envelope-template")]
    public string? EnvelopeTemplate { get; set; }

    [BooleanCommandSwitch("--no-encryption-protocols")]
    public bool? NoEncryptionProtocols { get; set; }
}