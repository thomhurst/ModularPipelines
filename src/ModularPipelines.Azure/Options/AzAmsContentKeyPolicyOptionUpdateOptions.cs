using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ams", "content-key-policy", "option", "update")]
public record AzAmsContentKeyPolicyOptionUpdateOptions(
[property: CommandSwitch("--policy-option-id")] string PolicyOptionId
) : AzOptions
{
    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--add-alt-token-key")]
    public string? AddAltTokenKey { get; set; }

    [CommandSwitch("--add-alt-token-key-type")]
    public string? AddAltTokenKeyType { get; set; }

    [CommandSwitch("--ask")]
    public string? Ask { get; set; }

    [CommandSwitch("--audience")]
    public string? Audience { get; set; }

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

    [CommandSwitch("--play-ready-template")]
    public string? PlayReadyTemplate { get; set; }

    [CommandSwitch("--policy-option-name")]
    public string? PolicyOptionName { get; set; }

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