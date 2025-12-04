using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "perimeter", "association", "create")]
public record AzNetworkPerimeterAssociationCreateOptions(
[property: CliOption("--association-name")] string AssociationName,
[property: CliOption("--perimeter-name")] string PerimeterName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--access-mode")]
    public string? AccessMode { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--private-link-resource")]
    public string? PrivateLinkResource { get; set; }

    [CliOption("--profile")]
    public string? Profile { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}