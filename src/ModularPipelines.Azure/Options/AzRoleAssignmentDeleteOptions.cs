using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("role", "assignment", "delete")]
public record AzRoleAssignmentDeleteOptions : AzOptions
{
    [CommandSwitch("--assignee")]
    public string? Assignee { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--include-inherited")]
    public bool? IncludeInherited { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--role")]
    public string? Role { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }

    [CommandSwitch("--yes")]
    public bool? Yes { get; set; } = true;
}