using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("confcom", "katapolicygen")]
public record AzConfcomKatapolicygenOptions(
[property: CommandSwitch("--yaml")] string Yaml
) : AzOptions
{
    [CommandSwitch("--config-map-file")]
    public string? ConfigMapFile { get; set; }

    [BooleanCommandSwitch("--outraw")]
    public bool? Outraw { get; set; }

    [BooleanCommandSwitch("--print-policy")]
    public bool? PrintPolicy { get; set; }

    [CommandSwitch("--settings-file-name")]
    public string? SettingsFileName { get; set; }

    [BooleanCommandSwitch("--use-cached-files")]
    public bool? UseCachedFiles { get; set; }
}