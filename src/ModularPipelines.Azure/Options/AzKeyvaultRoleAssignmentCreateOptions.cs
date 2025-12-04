using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("keyvault", "role", "assignment", "create")]
public record AzKeyvaultRoleAssignmentCreateOptions(
[property: CliOption("--role")] string Role,
[property: CliOption("--scope")] string Scope
) : AzOptions
{
    [CliOption("--assignee")]
    public string? Assignee { get; set; }

    [CliOption("--assignee-object-id")]
    public string? AssigneeObjectId { get; set; }

    [CliOption("--assignee-principal-type")]
    public string? AssigneePrincipalType { get; set; }

    [CliOption("--hsm-name")]
    public string? HsmName { get; set; }

    [CliOption("--id")]
    public string? Id { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }
}