using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "alb", "association", "create")]
public record AzNetworkAlbAssociationCreateOptions(
[property: CliOption("--alb-name")] string AlbName,
[property: CliOption("--association-name")] string AssociationName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--association-type")]
    public string? AssociationType { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--vnet-name")]
    public string? VnetName { get; set; }
}