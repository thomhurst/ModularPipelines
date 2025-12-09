using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse", "role", "assignment", "create")]
public record AzSynapseRoleAssignmentCreateOptions(
[property: CliOption("--role")] string Role,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--assignee")]
    public string? Assignee { get; set; }

    [CliOption("--assignee-object-id")]
    public string? AssigneeObjectId { get; set; }

    [CliOption("--assignee-principal-type")]
    public string? AssigneePrincipalType { get; set; }

    [CliOption("--assignment-id")]
    public string? AssignmentId { get; set; }

    [CliOption("--item")]
    public string? Item { get; set; }

    [CliOption("--item-type")]
    public string? ItemType { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }
}