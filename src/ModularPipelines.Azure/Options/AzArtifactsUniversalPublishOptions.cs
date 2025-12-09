using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("artifacts", "universal", "publish")]
public record AzArtifactsUniversalPublishOptions(
[property: CliOption("--feed")] string Feed,
[property: CliOption("--name")] string Name,
[property: CliOption("--path")] string Path,
[property: CliOption("--version")] string Version
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }
}