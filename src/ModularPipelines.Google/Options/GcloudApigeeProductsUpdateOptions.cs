using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigee", "products", "update")]
public record GcloudApigeeProductsUpdateOptions(
[property: PositionalArgument] string Product,
[property: PositionalArgument] string Organization
) : GcloudOptions
{
    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [BooleanCommandSwitch("--all-apis")]
    public bool? AllApis { get; set; }

    [CommandSwitch("--add-api")]
    public string[]? AddApi { get; set; }

    [CommandSwitch("--remove-api")]
    public string[]? RemoveApi { get; set; }

    [BooleanCommandSwitch("--all-environments")]
    public bool? AllEnvironments { get; set; }

    [CommandSwitch("--add-environment")]
    public string[]? AddEnvironment { get; set; }

    [CommandSwitch("--remove-environment")]
    public string[]? RemoveEnvironment { get; set; }

    [BooleanCommandSwitch("--all-resources")]
    public bool? AllResources { get; set; }

    [CommandSwitch("--add-resource")]
    public string[]? AddResource { get; set; }

    [CommandSwitch("--remove-resource")]
    public string[]? RemoveResource { get; set; }

    [BooleanCommandSwitch("--automatic-approval")]
    public bool? AutomaticApproval { get; set; }

    [BooleanCommandSwitch("--manual-approval")]
    public bool? ManualApproval { get; set; }

    [BooleanCommandSwitch("--clear-attributes")]
    public bool? ClearAttributes { get; set; }

    [CommandSwitch("--add-attribute")]
    public string[]? AddAttribute { get; set; }

    [CommandSwitch("--remove-attribute")]
    public string[]? RemoveAttribute { get; set; }

    [BooleanCommandSwitch("--clear-description")]
    public bool? ClearDescription { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--clear-oauth-scopes")]
    public bool? ClearOauthScopes { get; set; }

    [CommandSwitch("--add-oauth-scope")]
    public string[]? AddOauthScope { get; set; }

    [CommandSwitch("--remove-oauth-scope")]
    public string[]? RemoveOauthScope { get; set; }

    [BooleanCommandSwitch("--clear-quota")]
    public bool? ClearQuota { get; set; }

    [CommandSwitch("--quota")]
    public string? Quota { get; set; }

    [CommandSwitch("--quota-interval")]
    public string? QuotaInterval { get; set; }

    [CommandSwitch("--quota-unit")]
    public string? QuotaUnit { get; set; }

    [BooleanCommandSwitch("--internal-access")]
    public bool? InternalAccess { get; set; }

    [BooleanCommandSwitch("--private-access")]
    public bool? PrivateAccess { get; set; }

    [BooleanCommandSwitch("--public-access")]
    public bool? PublicAccess { get; set; }
}