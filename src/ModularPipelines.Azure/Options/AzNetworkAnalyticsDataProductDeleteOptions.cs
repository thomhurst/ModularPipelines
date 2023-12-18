using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-analytics", "data-product", "delete")]
public record AzNetworkAnalyticsDataProductDeleteOptions(
[property: CommandSwitch("--data-type-scope")] string DataTypeScope,
[property: CommandSwitch("--principal-id")] string PrincipalId,
[property: CommandSwitch("--principal-type")] string PrincipalType,
[property: CommandSwitch("--role")] string Role,
[property: CommandSwitch("--role-assignment-id")] string RoleAssignmentId,
[property: CommandSwitch("--role-id")] string RoleId,
[property: CommandSwitch("--user-name")] string UserName
) : AzOptions
{
    [CommandSwitch("--data-product-name")]
    public string? DataProductName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}

