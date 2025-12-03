using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "sql", "role", "assignment", "create")]
public record AzCosmosdbSqlRoleAssignmentCreateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--principal-id")] string PrincipalId,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--scope")] string Scope
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--role-assignment-id")]
    public string? RoleAssignmentId { get; set; }

    [CliOption("--role-definition-id")]
    public string? RoleDefinitionId { get; set; }

    [CliOption("--role-definition-name")]
    public string? RoleDefinitionName { get; set; }
}