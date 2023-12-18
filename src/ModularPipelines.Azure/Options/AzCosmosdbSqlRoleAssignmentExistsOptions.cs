using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "sql", "role", "assignment", "exists")]
public record AzCosmosdbSqlRoleAssignmentExistsOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--role-assignment-id")] string RoleAssignmentId
) : AzOptions;