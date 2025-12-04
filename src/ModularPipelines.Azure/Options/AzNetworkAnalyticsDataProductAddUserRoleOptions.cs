using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network-analytics", "data-product", "add-user-role")]
public record AzNetworkAnalyticsDataProductAddUserRoleOptions(
[property: CliOption("--data-type-scope")] string DataTypeScope,
[property: CliOption("--principal-id")] string PrincipalId,
[property: CliOption("--principal-type")] string PrincipalType,
[property: CliOption("--role")] string Role,
[property: CliOption("--role-id")] string RoleId,
[property: CliOption("--user-name")] string UserName
) : AzOptions
{
    [CliOption("--data-product-name")]
    public string? DataProductName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}