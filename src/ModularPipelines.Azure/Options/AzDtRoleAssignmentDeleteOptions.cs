using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dt", "role-assignment", "delete")]
public record AzDtRoleAssignmentDeleteOptions(
[property: CommandSwitch("--dt-name")] string DtName
) : AzOptions
{
    [CommandSwitch("--assignee")]
    public string? Assignee { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--role")]
    public string? Role { get; set; }
}