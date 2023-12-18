using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("role", "assignment", "list")]
public record AzRoleAssignmentListOptions : AzOptions
{
    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [CommandSwitch("--assignee")]
    public string? Assignee { get; set; }

    [BooleanCommandSwitch("--include-classic-administrators")]
    public bool? IncludeClassicAdministrators { get; set; }

    [BooleanCommandSwitch("--include-groups")]
    public bool? IncludeGroups { get; set; }

    [BooleanCommandSwitch("--include-inherited")]
    public bool? IncludeInherited { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--role")]
    public string? Role { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }
}