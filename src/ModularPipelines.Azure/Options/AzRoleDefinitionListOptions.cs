using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("role", "definition", "list")]
public record AzRoleDefinitionListOptions(
[property: CommandSwitch("--role-definition")] string RoleDefinition
) : AzOptions
{
    [BooleanCommandSwitch("--custom-role-only")]
    public bool? CustomRoleOnly { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }
}