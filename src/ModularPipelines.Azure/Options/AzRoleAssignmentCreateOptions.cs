using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("role", "assignment", "create")]
public record AzRoleAssignmentCreateOptions(
[property: CommandSwitch("--role")] string Role,
[property: CommandSwitch("--scope")] string Scope
) : AzOptions
{
    [CommandSwitch("--assignee")]
    public string? Assignee { get; set; }

    [CommandSwitch("--assignee-object-id")]
    public string? AssigneeObjectId { get; set; }

    [CommandSwitch("--assignee-principal-type")]
    public string? AssigneePrincipalType { get; set; }

    [CommandSwitch("--condition")]
    public string? Condition { get; set; }

    [CommandSwitch("--condition-version")]
    public string? ConditionVersion { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }
}