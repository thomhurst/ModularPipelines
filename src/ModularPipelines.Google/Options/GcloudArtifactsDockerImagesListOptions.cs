using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "docker", "images", "list")]
public record GcloudArtifactsDockerImagesListOptions(
[property: CliArgument] string ImagePath
) : GcloudOptions
{
    [CliFlag("--include-tags")]
    public bool? IncludeTags { get; set; }

    [CliOption("--occurrence-filter")]
    public string? OccurrenceFilter { get; set; }

    [CliFlag("--show-occurrences")]
    public bool? ShowOccurrences { get; set; }

    [CliOption("--show-occurrences-from")]
    public string? ShowOccurrencesFrom { get; set; }
}