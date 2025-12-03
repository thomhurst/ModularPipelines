using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dt", "route", "create")]
public record AzDtRouteCreateOptions(
[property: CliOption("--dt-name")] string DtName,
[property: CliOption("--en")] string En,
[property: CliOption("--rn")] string Rn
) : AzOptions
{
    [CliFlag("--filter")]
    public bool? Filter { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}