using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dt", "route", "delete")]
public record AzDtRouteDeleteOptions(
[property: CliOption("--dt-name")] string DtName,
[property: CliOption("--rn")] string Rn
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}