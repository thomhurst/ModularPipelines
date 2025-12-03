using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("keyvault", "role", "definition", "show")]
public record AzKeyvaultRoleDefinitionShowOptions(
[property: CliOption("--hsm-name")] string HsmName
) : AzOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--role-id")]
    public string? RoleId { get; set; }
}