using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("keyvault", "role", "definition", "delete")]
public record AzKeyvaultRoleDefinitionDeleteOptions(
[property: CliOption("--hsm-name")] string HsmName
) : AzOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--role-id")]
    public string? RoleId { get; set; }
}