using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "role", "assignment", "list")]
public record AzSynapseRoleAssignmentListOptions(
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CommandSwitch("--assignee")]
    public string? Assignee { get; set; }

    [CommandSwitch("--assignee-object-id")]
    public string? AssigneeObjectId { get; set; }

    [CommandSwitch("--item")]
    public string? Item { get; set; }

    [CommandSwitch("--item-type")]
    public string? ItemType { get; set; }

    [CommandSwitch("--role")]
    public string? Role { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }
}