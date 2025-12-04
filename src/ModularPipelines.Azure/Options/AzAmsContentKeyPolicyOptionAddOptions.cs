using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ams", "content-key-policy", "option", "add")]
public record AzAmsContentKeyPolicyOptionAddOptions(
[property: CliOption("--policy-option-name")] string PolicyOptionName
) : AzOptions
{
    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--alt-rsa-token-keys")]
    public string? AltRsaTokenKeys { get; set; }

    [CliOption("--alt-symmetric-token-keys")]
    public string? AltSymmetricTokenKeys { get; set; }

    [CliOption("--alt-x509-token-keys")]
    public string? AltX509TokenKeys { get; set; }

    [CliOption("--ask")]
    public string? Ask { get; set; }

    [CliOption("--audience")]
    public string? Audience { get; set; }

    [CliFlag("--clear-key-configuration")]
    public bool? ClearKeyConfiguration { get; set; }

    [CliOption("--fair-play-pfx")]
    public string? FairPlayPfx { get; set; }

    [CliOption("--fair-play-pfx-password")]
    public string? FairPlayPfxPassword { get; set; }

    [CliOption("--fp-playback-duration-seconds")]
    public string? FpPlaybackDurationSeconds { get; set; }

    [CliOption("--fp-storage-duration-seconds")]
    public string? FpStorageDurationSeconds { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--issuer")]
    public string? Issuer { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--open-id-connect-discovery-document")]
    public string? OpenIdConnectDiscoveryDocument { get; set; }

    [CliFlag("--open-restriction")]
    public bool? OpenRestriction { get; set; }

    [CliOption("--play-ready-template")]
    public string? PlayReadyTemplate { get; set; }

    [CliOption("--rental-and-lease-key-type")]
    public string? RentalAndLeaseKeyType { get; set; }

    [CliOption("--rental-duration")]
    public string? RentalDuration { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--token-claims")]
    public string? TokenClaims { get; set; }

    [CliOption("--token-key")]
    public string? TokenKey { get; set; }

    [CliOption("--token-key-type")]
    public string? TokenKeyType { get; set; }

    [CliOption("--token-type")]
    public string? TokenType { get; set; }

    [CliOption("--widevine-template")]
    public string? WidevineTemplate { get; set; }
}