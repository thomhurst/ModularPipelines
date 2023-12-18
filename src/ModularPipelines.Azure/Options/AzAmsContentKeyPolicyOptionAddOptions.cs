using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ams", "content-key-policy", "option", "add")]
public record AzAmsContentKeyPolicyOptionAddOptions(
[property: CommandSwitch("--policy-option-name")] string PolicyOptionName
) : AzOptions
{
    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--alt-rsa-token-keys")]
    public string? AltRsaTokenKeys { get; set; }

    [CommandSwitch("--alt-symmetric-token-keys")]
    public string? AltSymmetricTokenKeys { get; set; }

    [CommandSwitch("--alt-x509-token-keys")]
    public string? AltX509TokenKeys { get; set; }

    [CommandSwitch("--ask")]
    public string? Ask { get; set; }

    [CommandSwitch("--audience")]
    public string? Audience { get; set; }

    [BooleanCommandSwitch("--clear-key-configuration")]
    public bool? ClearKeyConfiguration { get; set; }

    [CommandSwitch("--fair-play-pfx")]
    public string? FairPlayPfx { get; set; }

    [CommandSwitch("--fair-play-pfx-password")]
    public string? FairPlayPfxPassword { get; set; }

    [CommandSwitch("--fp-playback-duration-seconds")]
    public string? FpPlaybackDurationSeconds { get; set; }

    [CommandSwitch("--fp-storage-duration-seconds")]
    public string? FpStorageDurationSeconds { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--issuer")]
    public string? Issuer { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--open-id-connect-discovery-document")]
    public string? OpenIdConnectDiscoveryDocument { get; set; }

    [BooleanCommandSwitch("--open-restriction")]
    public bool? OpenRestriction { get; set; }

    [CommandSwitch("--play-ready-template")]
    public string? PlayReadyTemplate { get; set; }

    [CommandSwitch("--rental-and-lease-key-type")]
    public string? RentalAndLeaseKeyType { get; set; }

    [CommandSwitch("--rental-duration")]
    public string? RentalDuration { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--token-claims")]
    public string? TokenClaims { get; set; }

    [CommandSwitch("--token-key")]
    public string? TokenKey { get; set; }

    [CommandSwitch("--token-key-type")]
    public string? TokenKeyType { get; set; }

    [CommandSwitch("--token-type")]
    public string? TokenType { get; set; }

    [CommandSwitch("--widevine-template")]
    public string? WidevineTemplate { get; set; }
}