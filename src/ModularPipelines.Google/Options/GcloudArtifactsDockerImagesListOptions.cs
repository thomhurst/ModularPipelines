using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("artifacts", "docker", "images", "list")]
public record GcloudArtifactsDockerImagesListOptions(
[property: PositionalArgument] string ImagePath
) : GcloudOptions
{
    [BooleanCommandSwitch("--include-tags")]
    public bool? IncludeTags { get; set; }

    [CommandSwitch("--occurrence-filter")]
    public string? OccurrenceFilter { get; set; }

    [BooleanCommandSwitch("--show-occurrences")]
    public bool? ShowOccurrences { get; set; }

    [CommandSwitch("--show-occurrences-from")]
    public string? ShowOccurrencesFrom { get; set; }
}