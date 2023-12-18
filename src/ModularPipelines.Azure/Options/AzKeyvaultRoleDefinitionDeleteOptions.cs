using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "role", "definition", "delete")]
public record AzKeyvaultRoleDefinitionDeleteOptions(
[property: CommandSwitch("--hsm-name")] string HsmName
) : AzOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--role-id")]
    public string? RoleId { get; set; }
}