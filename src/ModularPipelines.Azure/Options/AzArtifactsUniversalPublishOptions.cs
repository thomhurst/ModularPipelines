using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("artifacts", "universal", "publish")]
public record AzArtifactsUniversalPublishOptions(
[property: CommandSwitch("--feed")] string Feed,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--path")] string Path,
[property: CommandSwitch("--version")] string Version
) : AzOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--project")]
    public string? Project { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }
}

