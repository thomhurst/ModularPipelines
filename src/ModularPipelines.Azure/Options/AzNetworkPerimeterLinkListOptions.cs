using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "perimeter", "link", "list")]
public record AzNetworkPerimeterLinkListOptions(
[property: CliOption("--perimeter-name")] string PerimeterName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--skip-token")]
    public string? SkipToken { get; set; }

    [CliOption("--top")]
    public string? Top { get; set; }
}