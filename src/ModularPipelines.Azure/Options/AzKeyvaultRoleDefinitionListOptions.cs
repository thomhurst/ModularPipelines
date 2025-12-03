using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("keyvault", "role", "definition", "list")]
public record AzKeyvaultRoleDefinitionListOptions : AzOptions
{
    [CliFlag("--custom-role-only")]
    public bool? CustomRoleOnly { get; set; }

    [CliOption("--hsm-name")]
    public string? HsmName { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }
}