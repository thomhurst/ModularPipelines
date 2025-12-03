using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dt", "route", "show")]
public record AzDtRouteShowOptions(
[property: CliOption("--dt-name")] string DtName,
[property: CliOption("--rn")] string Rn
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}