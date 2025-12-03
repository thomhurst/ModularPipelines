using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pipelines", "runs", "artifact", "download")]
public record AzPipelinesRunsArtifactDownloadOptions(
[property: CliOption("--artifact-name")] string ArtifactName,
[property: CliOption("--path")] string Path,
[property: CliOption("--run-id")] string RunId
) : AzOptions
{
    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }
}