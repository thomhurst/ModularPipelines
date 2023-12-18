using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "role", "assignment", "create")]
public record AzSynapseRoleAssignmentCreateOptions(
[property: CommandSwitch("--role")] string Role,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CommandSwitch("--assignee")]
    public string? Assignee { get; set; }

    [CommandSwitch("--assignee-object-id")]
    public string? AssigneeObjectId { get; set; }

    [CommandSwitch("--assignee-principal-type")]
    public string? AssigneePrincipalType { get; set; }

    [CommandSwitch("--assignment-id")]
    public string? AssignmentId { get; set; }

    [CommandSwitch("--item")]
    public string? Item { get; set; }

    [CommandSwitch("--item-type")]
    public string? ItemType { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }
}