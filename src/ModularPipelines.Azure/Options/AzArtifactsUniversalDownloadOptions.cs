using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("artifacts", "universal", "download")]
public record AzArtifactsUniversalDownloadOptions(
[property: CommandSwitch("--feed")] string Feed,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--path")] string Path,
[property: CommandSwitch("--version")] string Version
) : AzOptions
{
    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [CommandSwitch("--file-filter")]
    public string? FileFilter { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--project")]
    public string? Project { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }
}