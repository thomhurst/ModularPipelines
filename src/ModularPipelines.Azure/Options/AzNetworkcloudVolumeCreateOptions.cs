using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("networkcloud", "volume", "create")]
public record AzNetworkcloudVolumeCreateOptions(
[property: CliOption("--extended-location")] string ExtendedLocation,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--size")] string Size
) : AzOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}