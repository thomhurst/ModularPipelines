using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dt", "model", "update")]
public record AzDtModelUpdateOptions(
[property: CliOption("--dt-name")] string DtName,
[property: CliOption("--dtmi")] string Dtmi
) : AzOptions
{
    [CliFlag("--decommission")]
    public bool? Decommission { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}