using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("aosm", "nfd", "publish")]
public record AzAosmNfdPublishOptions(
[property: CliOption("--config-file")] string ConfigFile,
[property: CliOption("--definition-type")] string DefinitionType
) : AzOptions
{
    [CliOption("--definition-file")]
    public string? DefinitionFile { get; set; }

    [CliOption("--manifest-file")]
    public string? ManifestFile { get; set; }

    [CliOption("--manifest-params-file")]
    public string? ManifestParamsFile { get; set; }

    [CliFlag("--no-subscription-permissions")]
    public bool? NoSubscriptionPermissions { get; set; }

    [CliOption("--parameters-file")]
    public string? ParametersFile { get; set; }

    [CliOption("--skip")]
    public string? Skip { get; set; }
}