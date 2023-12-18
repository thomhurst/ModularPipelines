using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "role", "definition", "list")]
public record AzKeyvaultRoleDefinitionListOptions(
[property: CommandSwitch("--hsm-name")] string HsmName
) : AzOptions
{
    [BooleanCommandSwitch("--custom-role-only")]
    public bool? CustomRoleOnly { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }
}

