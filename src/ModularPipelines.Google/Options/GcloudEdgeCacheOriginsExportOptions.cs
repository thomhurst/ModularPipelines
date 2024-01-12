using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cache", "origins", "export")]
public record GcloudEdgeCacheOriginsExportOptions(
[property: PositionalArgument] string Origin,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--destination")]
    public string? Destination { get; set; }
}