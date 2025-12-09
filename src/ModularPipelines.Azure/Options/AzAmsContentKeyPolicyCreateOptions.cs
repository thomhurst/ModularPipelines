using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ams", "content-key-policy", "create")]
public record AzAmsContentKeyPolicyCreateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--name")] string Name,
[property: CliOption("--policy-option-name")] string PolicyOptionName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
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

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--fair-play-pfx")]
    public string? FairPlayPfx { get; set; }

    [CliOption("--fair-play-pfx-password")]
    public string? FairPlayPfxPassword { get; set; }

    [CliOption("--fp-playback-duration-seconds")]
    public string? FpPlaybackDurationSeconds { get; set; }

    [CliOption("--fp-storage-duration-seconds")]
    public string? FpStorageDurationSeconds { get; set; }

    [CliOption("--issuer")]
    public string? Issuer { get; set; }

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