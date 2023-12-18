using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dt", "role-assignment", "create")]
public record AzDtRoleAssignmentCreateOptions(
[property: CommandSwitch("--assignee")] string Assignee,
[property: CommandSwitch("--dt-name")] string DtName,
[property: CommandSwitch("--role")] string Role
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}