using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigee", "products", "update")]
public record GcloudApigeeProductsUpdateOptions(
[property: CliArgument] string Product,
[property: CliArgument] string Organization
) : GcloudOptions
{
    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliFlag("--all-apis")]
    public bool? AllApis { get; set; }

    [CliOption("--add-api")]
    public string[]? AddApi { get; set; }

    [CliOption("--remove-api")]
    public string[]? RemoveApi { get; set; }

    [CliFlag("--all-environments")]
    public bool? AllEnvironments { get; set; }

    [CliOption("--add-environment")]
    public string[]? AddEnvironment { get; set; }

    [CliOption("--remove-environment")]
    public string[]? RemoveEnvironment { get; set; }

    [CliFlag("--all-resources")]
    public bool? AllResources { get; set; }

    [CliOption("--add-resource")]
    public string[]? AddResource { get; set; }

    [CliOption("--remove-resource")]
    public string[]? RemoveResource { get; set; }

    [CliFlag("--automatic-approval")]
    public bool? AutomaticApproval { get; set; }

    [CliFlag("--manual-approval")]
    public bool? ManualApproval { get; set; }

    [CliFlag("--clear-attributes")]
    public bool? ClearAttributes { get; set; }

    [CliOption("--add-attribute")]
    public string[]? AddAttribute { get; set; }

    [CliOption("--remove-attribute")]
    public string[]? RemoveAttribute { get; set; }

    [CliFlag("--clear-description")]
    public bool? ClearDescription { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--clear-oauth-scopes")]
    public bool? ClearOauthScopes { get; set; }

    [CliOption("--add-oauth-scope")]
    public string[]? AddOauthScope { get; set; }

    [CliOption("--remove-oauth-scope")]
    public string[]? RemoveOauthScope { get; set; }

    [CliFlag("--clear-quota")]
    public bool? ClearQuota { get; set; }

    [CliOption("--quota")]
    public string? Quota { get; set; }

    [CliOption("--quota-interval")]
    public string? QuotaInterval { get; set; }

    [CliOption("--quota-unit")]
    public string? QuotaUnit { get; set; }

    [CliFlag("--internal-access")]
    public bool? InternalAccess { get; set; }

    [CliFlag("--private-access")]
    public bool? PrivateAccess { get; set; }

    [CliFlag("--public-access")]
    public bool? PublicAccess { get; set; }
}