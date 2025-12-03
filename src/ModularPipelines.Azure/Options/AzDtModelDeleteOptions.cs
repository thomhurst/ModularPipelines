using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dt", "model", "delete")]
public record AzDtModelDeleteOptions(
[property: CliOption("--dt-name")] string DtName,
[property: CliOption("--dtmi")] string Dtmi
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}