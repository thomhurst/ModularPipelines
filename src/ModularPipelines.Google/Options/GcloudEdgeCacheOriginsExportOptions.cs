using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cache", "origins", "export")]
public record GcloudEdgeCacheOriginsExportOptions(
[property: CliArgument] string Origin,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--destination")]
    public string? Destination { get; set; }
}