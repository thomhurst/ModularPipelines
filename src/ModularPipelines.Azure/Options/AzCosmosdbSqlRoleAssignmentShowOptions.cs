using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cosmosdb", "sql", "role", "assignment", "show")]
public record AzCosmosdbSqlRoleAssignmentShowOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--role-assignment-id")] string RoleAssignmentId
) : AzOptions;