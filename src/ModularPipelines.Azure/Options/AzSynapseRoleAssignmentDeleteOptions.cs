using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("synapse", "role", "assignment", "delete")]
public record AzSynapseRoleAssignmentDeleteOptions(
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--assignee")]
    public string? Assignee { get; set; }

    [CliOption("--assignee-object-id")]
    public string? AssigneeObjectId { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--item")]
    public string? Item { get; set; }

    [CliOption("--item-type")]
    public string? ItemType { get; set; }

    [CliOption("--role")]
    public string? Role { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}