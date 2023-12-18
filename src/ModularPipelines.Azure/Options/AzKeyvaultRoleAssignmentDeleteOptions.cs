using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "role", "assignment", "delete")]
public record AzKeyvaultRoleAssignmentDeleteOptions : AzOptions
{
    [CommandSwitch("--assignee")]
    public string? Assignee { get; set; }

    [CommandSwitch("--assignee-object-id")]
    public string? AssigneeObjectId { get; set; }

    [CommandSwitch("--hsm-name")]
    public string? HsmName { get; set; }

    [CommandSwitch("--id")]
    public string? Id { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--role")]
    public string? Role { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }
}

