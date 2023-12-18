using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "app", "show")]
public record AzAdAppShowOptions(
[property: CommandSwitch("--id")] string Id
) : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--app-roles")]
    public string? AppRoles { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [BooleanCommandSwitch("--enable-access-token-issuance")]
    public bool? EnableAccessTokenIssuance { get; set; }

    [BooleanCommandSwitch("--enable-id-token-issuance")]
    public bool? EnableIdTokenIssuance { get; set; }

    [CommandSwitch("--end-date")]
    public string? EndDate { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--identifier-uris")]
    public string? IdentifierUris { get; set; }

    [BooleanCommandSwitch("--is-fallback-public-client")]
    public bool? IsFallbackPublicClient { get; set; }

    [CommandSwitch("--key-display-name")]
    public string? KeyDisplayName { get; set; }

    [CommandSwitch("--key-type")]
    public string? KeyType { get; set; }

    [CommandSwitch("--key-usage")]
    public string? KeyUsage { get; set; }

    [CommandSwitch("--key-value")]
    public string? KeyValue { get; set; }

    [CommandSwitch("--optional-claims")]
    public string? OptionalClaims { get; set; }

    [CommandSwitch("--public-client-redirect-uris")]
    public string? PublicClientRedirectUris { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--required-resource-accesses")]
    public string? RequiredResourceAccesses { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--sign-in-audience")]
    public string? SignInAudience { get; set; }

    [CommandSwitch("--start-date")]
    public string? StartDate { get; set; }

    [CommandSwitch("--web-home-page-url")]
    public string? WebHomePageUrl { get; set; }

    [CommandSwitch("--web-redirect-uris")]
    public string? WebRedirectUris { get; set; }
}

