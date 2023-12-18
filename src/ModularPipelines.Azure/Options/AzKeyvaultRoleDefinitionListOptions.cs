using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "role", "definition", "list")]
public record AzKeyvaultRoleDefinitionListOptions : AzOptions
{
    [BooleanCommandSwitch("--custom-role-only")]
    public bool? CustomRoleOnly { get; set; }

    [CommandSwitch("--hsm-name")]
    public string? HsmName { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }
}