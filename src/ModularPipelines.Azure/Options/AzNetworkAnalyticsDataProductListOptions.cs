using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-analytics", "data-product", "list")]
public record AzNetworkAnalyticsDataProductListOptions(
[property: CommandSwitch("--data-type-scope")] string DataTypeScope,
[property: CommandSwitch("--principal-id")] string PrincipalId,
[property: CommandSwitch("--principal-type")] string PrincipalType,
[property: CommandSwitch("--role")] string Role,
[property: CommandSwitch("--role-assignment-id")] string RoleAssignmentId,
[property: CommandSwitch("--role-id")] string RoleId,
[property: CommandSwitch("--user-name")] string UserName
) : AzOptions
{
    [CommandSwitch("--max-items")]
    public string? MaxItems { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}