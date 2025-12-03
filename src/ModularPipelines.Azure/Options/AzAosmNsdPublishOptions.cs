using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("aosm", "nsd", "publish")]
public record AzAosmNsdPublishOptions(
[property: CliOption("--config-file")] string ConfigFile
) : AzOptions
{
    [CliOption("--design-file")]
    public string? DesignFile { get; set; }

    [CliOption("--manifest-file")]
    public string? ManifestFile { get; set; }

    [CliOption("--manifest-params-file")]
    public string? ManifestParamsFile { get; set; }

    [CliOption("--parameters-json-file")]
    public string? ParametersJsonFile { get; set; }

    [CliOption("--skip")]
    public string? Skip { get; set; }
}