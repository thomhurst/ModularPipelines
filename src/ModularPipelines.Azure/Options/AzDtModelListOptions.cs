using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dt", "model", "list")]
public record AzDtModelListOptions(
[property: CliOption("--dt-name")] string DtName
) : AzOptions
{
    [CliFlag("--def")]
    public bool? Def { get; set; }

    [CliOption("--dependencies-for")]
    public string? DependenciesFor { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}