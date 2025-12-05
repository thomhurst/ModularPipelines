using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cosmosdb", "sql", "role", "assignment", "update")]
public record AzCosmosdbSqlRoleAssignmentUpdateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--role-assignment-id")] string RoleAssignmentId
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--principal-id")]
    public string? PrincipalId { get; set; }

    [CliOption("--role-definition-id")]
    public string? RoleDefinitionId { get; set; }

    [CliOption("--role-definition-name")]
    public string? RoleDefinitionName { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }
}