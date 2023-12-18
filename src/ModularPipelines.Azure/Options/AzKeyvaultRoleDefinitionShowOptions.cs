using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "role", "definition", "show")]
public record AzKeyvaultRoleDefinitionShowOptions(
[property: CommandSwitch("--hsm-name")] string HsmName
) : AzOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--role-id")]
    public string? RoleId { get; set; }
}