using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("role", "assignment", "create")]
public record AzRoleAssignmentCreateOptions(
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

    [CliOption("--condition")]
    public string? Condition { get; set; }

    [CliOption("--condition-version")]
    public string? ConditionVersion { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }
}