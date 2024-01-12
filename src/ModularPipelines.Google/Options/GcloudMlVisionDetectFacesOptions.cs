using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "vision", "detect-faces")]
public record GcloudMlVisionDetectFacesOptions(
[property: PositionalArgument] string ImagePath
) : GcloudOptions
{
    [CommandSwitch("--max-results")]
    public string? MaxResults { get; set; }
}