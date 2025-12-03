using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "sql", "role", "assignment", "exists")]
public record AzCosmosdbSqlRoleAssignmentExistsOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--role-assignment-id")] string RoleAssignmentId
) : AzOptions;