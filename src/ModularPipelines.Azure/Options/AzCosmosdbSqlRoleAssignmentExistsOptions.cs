using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "sql", "role", "assignment", "exists")]
public record AzCosmosdbSqlRoleAssignmentExistsOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--role-assignment-id")] string RoleAssignmentId
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--principal-id")]
    public string? PrincipalId { get; set; }

    [CommandSwitch("--role-definition-id")]
    public string? RoleDefinitionId { get; set; }

    [CommandSwitch("--role-definition-name")]
    public string? RoleDefinitionName { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }
}

