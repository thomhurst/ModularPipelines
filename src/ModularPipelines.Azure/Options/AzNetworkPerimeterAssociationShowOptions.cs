using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "perimeter", "association", "show")]
public record AzNetworkPerimeterAssociationShowOptions : AzOptions
{
    [CliOption("--association-name")]
    public string? AssociationName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--perimeter-name")]
    public string? PerimeterName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}