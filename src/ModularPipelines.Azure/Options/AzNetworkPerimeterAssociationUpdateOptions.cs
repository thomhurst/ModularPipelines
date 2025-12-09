using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "perimeter", "association", "update")]
public record AzNetworkPerimeterAssociationUpdateOptions : AzOptions
{
    [CliOption("--access-mode")]
    public string? AccessMode { get; set; }

    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--association-name")]
    public string? AssociationName { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--perimeter-name")]
    public string? PerimeterName { get; set; }

    [CliOption("--private-link-resource")]
    public string? PrivateLinkResource { get; set; }

    [CliOption("--profile")]
    public string? Profile { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}