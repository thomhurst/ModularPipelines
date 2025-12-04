using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("role", "definition", "delete")]
public record AzRoleDefinitionDeleteOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliFlag("--custom-role-only")]
    public bool? CustomRoleOnly { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }
}