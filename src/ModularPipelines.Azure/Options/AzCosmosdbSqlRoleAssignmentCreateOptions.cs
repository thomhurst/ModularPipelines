using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "sql", "role", "assignment", "create")]
public record AzCosmosdbSqlRoleAssignmentCreateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--principal-id")] string PrincipalId,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--scope")] string Scope
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--role-assignment-id")]
    public string? RoleAssignmentId { get; set; }

    [CommandSwitch("--role-definition-id")]
    public string? RoleDefinitionId { get; set; }

    [CommandSwitch("--role-definition-name")]
    public string? RoleDefinitionName { get; set; }
}