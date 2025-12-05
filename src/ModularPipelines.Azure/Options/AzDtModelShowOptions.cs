using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dt", "model", "show")]
public record AzDtModelShowOptions(
[property: CliOption("--dt-name")] string DtName,
[property: CliOption("--dtmi")] string Dtmi
) : AzOptions
{
    [CliFlag("--def")]
    public bool? Def { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}