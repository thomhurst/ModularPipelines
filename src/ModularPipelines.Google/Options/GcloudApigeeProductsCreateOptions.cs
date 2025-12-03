using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigee", "products", "create")]
public record GcloudApigeeProductsCreateOptions(
[property: CliArgument] string InternalName,
[property: CliArgument] string Organization
) : GcloudOptions
{
    [CliOption("--attributes")]
    public string[]? Attributes { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliFlag("--manual-approval")]
    public bool? ManualApproval { get; set; }

    [CliOption("--oauth-scopes")]
    public string[]? OauthScopes { get; set; }

    [CliFlag("--all-environments")]
    public bool? AllEnvironments { get; set; }

    [CliOption("--environments")]
    public string[]? Environments { get; set; }

    [CliFlag("--all-proxies")]
    public bool? AllProxies { get; set; }

    [CliOption("--apis")]
    public string[]? Apis { get; set; }

    [CliOption("--resources")]
    public string[]? Resources { get; set; }

    [CliFlag("--internal-access")]
    public bool? InternalAccess { get; set; }

    [CliFlag("--private-access")]
    public bool? PrivateAccess { get; set; }

    [CliFlag("--public-access")]
    public bool? PublicAccess { get; set; }

    [CliOption("--quota")]
    public string? Quota { get; set; }

    [CliOption("--quota-interval")]
    public string? QuotaInterval { get; set; }

    [CliOption("--quota-unit")]
    public string? QuotaUnit { get; set; }
}