using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "alb", "association", "show")]
public record AzNetworkAlbAssociationShowOptions : AzOptions
{
    [CliOption("--alb-name")]
    public string? AlbName { get; set; }

    [CliOption("--association-name")]
    public string? AssociationName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}