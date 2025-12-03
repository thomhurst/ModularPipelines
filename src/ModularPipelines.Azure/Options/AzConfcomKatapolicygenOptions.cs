using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("confcom", "katapolicygen")]
public record AzConfcomKatapolicygenOptions(
[property: CliOption("--yaml")] string Yaml
) : AzOptions
{
    [CliOption("--config-map-file")]
    public string? ConfigMapFile { get; set; }

    [CliFlag("--outraw")]
    public bool? Outraw { get; set; }

    [CliFlag("--print-policy")]
    public bool? PrintPolicy { get; set; }

    [CliOption("--settings-file-name")]
    public string? SettingsFileName { get; set; }

    [CliFlag("--use-cached-files")]
    public bool? UseCachedFiles { get; set; }
}