using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cache", "services", "export")]
public record GcloudEdgeCacheServicesExportOptions(
[property: PositionalArgument] string Service,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--destination")]
    public string? Destination { get; set; }
}