using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigee", "products", "create")]
public record GcloudApigeeProductsCreateOptions(
[property: PositionalArgument] string InternalName,
[property: PositionalArgument] string Organization
) : GcloudOptions
{
    [CommandSwitch("--attributes")]
    public string[]? Attributes { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [BooleanCommandSwitch("--manual-approval")]
    public bool? ManualApproval { get; set; }

    [CommandSwitch("--oauth-scopes")]
    public string[]? OauthScopes { get; set; }

    [BooleanCommandSwitch("--all-environments")]
    public bool? AllEnvironments { get; set; }

    [CommandSwitch("--environments")]
    public string[]? Environments { get; set; }

    [BooleanCommandSwitch("--all-proxies")]
    public bool? AllProxies { get; set; }

    [CommandSwitch("--apis")]
    public string[]? Apis { get; set; }

    [CommandSwitch("--resources")]
    public string[]? Resources { get; set; }

    [BooleanCommandSwitch("--internal-access")]
    public bool? InternalAccess { get; set; }

    [BooleanCommandSwitch("--private-access")]
    public bool? PrivateAccess { get; set; }

    [BooleanCommandSwitch("--public-access")]
    public bool? PublicAccess { get; set; }

    [CommandSwitch("--quota")]
    public string? Quota { get; set; }

    [CommandSwitch("--quota-interval")]
    public string? QuotaInterval { get; set; }

    [CommandSwitch("--quota-unit")]
    public string? QuotaUnit { get; set; }
}