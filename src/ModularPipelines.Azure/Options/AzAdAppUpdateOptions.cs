using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ad", "app", "update")]
public record AzAdAppUpdateOptions(
[property: CliOption("--id")] string Id
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--app-roles")]
    public string? AppRoles { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliFlag("--enable-access-token-issuance")]
    public bool? EnableAccessTokenIssuance { get; set; }

    [CliFlag("--enable-id-token-issuance")]
    public bool? EnableIdTokenIssuance { get; set; }

    [CliOption("--end-date")]
    public string? EndDate { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--identifier-uris")]
    public string? IdentifierUris { get; set; }

    [CliFlag("--is-fallback-public-client")]
    public bool? IsFallbackPublicClient { get; set; }

    [CliOption("--key-display-name")]
    public string? KeyDisplayName { get; set; }

    [CliOption("--key-type")]
    public string? KeyType { get; set; }

    [CliOption("--key-usage")]
    public string? KeyUsage { get; set; }

    [CliOption("--key-value")]
    public string? KeyValue { get; set; }

    [CliOption("--optional-claims")]
    public string? OptionalClaims { get; set; }

    [CliOption("--public-client-redirect-uris")]
    public string? PublicClientRedirectUris { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--required-resource-accesses")]
    public string? RequiredResourceAccesses { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--sign-in-audience")]
    public string? SignInAudience { get; set; }

    [CliOption("--start-date")]
    public string? StartDate { get; set; }

    [CliOption("--web-home-page-url")]
    public string? WebHomePageUrl { get; set; }

    [CliOption("--web-redirect-uris")]
    public string? WebRedirectUris { get; set; }
}