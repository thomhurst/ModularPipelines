using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "universal", "download")]
public record AzArtifactsUniversalDownloadOptions(
[property: CliOption("--feed")] string Feed,
[property: CliOption("--name")] string Name,
[property: CliOption("--path")] string Path,
[property: CliOption("--version")] string Version
) : AzOptions
{
    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--file-filter")]
    public string? FileFilter { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }
}