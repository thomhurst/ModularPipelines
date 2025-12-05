using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "vnet", "tap", "create")]
public record AzNetworkVnetTapCreateOptions(
[property: CliOption("--destination")] string Destination,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--port")]
    public int? Port { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}