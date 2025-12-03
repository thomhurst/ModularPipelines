using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ams", "content-key-policy", "option", "update")]
public record AzAmsContentKeyPolicyOptionUpdateOptions(
[property: CliOption("--policy-option-id")] string PolicyOptionId
) : AzOptions
{
    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--add-alt-token-key")]
    public string? AddAltTokenKey { get; set; }

    [CliOption("--add-alt-token-key-type")]
    public string? AddAltTokenKeyType { get; set; }

    [CliOption("--ask")]
    public string? Ask { get; set; }

    [CliOption("--audience")]
    public string? Audience { get; set; }

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

    [CliOption("--play-ready-template")]
    public string? PlayReadyTemplate { get; set; }

    [CliOption("--policy-option-name")]
    public string? PolicyOptionName { get; set; }

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