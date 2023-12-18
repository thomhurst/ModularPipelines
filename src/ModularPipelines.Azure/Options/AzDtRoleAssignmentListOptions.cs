using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dt", "role-assignment", "list")]
public record AzDtRoleAssignmentListOptions(
[property: CommandSwitch("--dt-name")] string DtName
) : AzOptions
{
    [BooleanCommandSwitch("--include-inherited")]
    public bool? IncludeInherited { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--role")]
    public string? Role { get; set; }
}