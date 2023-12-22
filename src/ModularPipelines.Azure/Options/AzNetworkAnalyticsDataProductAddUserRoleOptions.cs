using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-analytics", "data-product", "add-user-role")]
public record AzNetworkAnalyticsDataProductAddUserRoleOptions(
[property: CommandSwitch("--data-type-scope")] string DataTypeScope,
[property: CommandSwitch("--principal-id")] string PrincipalId,
[property: CommandSwitch("--principal-type")] string PrincipalType,
[property: CommandSwitch("--role")] string Role,
[property: CommandSwitch("--role-id")] string RoleId,
[property: CommandSwitch("--user-name")] string UserName
) : AzOptions
{
    [CommandSwitch("--data-product-name")]
    public string? DataProductName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}