using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse", "role", "assignment", "list")]
public record AzSynapseRoleAssignmentListOptions(
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--assignee")]
    public string? Assignee { get; set; }

    [CliOption("--assignee-object-id")]
    public string? AssigneeObjectId { get; set; }

    [CliOption("--item")]
    public string? Item { get; set; }

    [CliOption("--item-type")]
    public string? ItemType { get; set; }

    [CliOption("--role")]
    public string? Role { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }
}