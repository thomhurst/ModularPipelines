using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "perimeter", "profile", "create")]
public record AzNetworkPerimeterProfileCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--perimeter-name")] string PerimeterName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}