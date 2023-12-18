using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aosm", "nfd", "publish")]
public record AzAosmNfdPublishOptions(
[property: CommandSwitch("--config-file")] string ConfigFile,
[property: CommandSwitch("--definition-type")] string DefinitionType
) : AzOptions
{
    [CommandSwitch("--definition-file")]
    public string? DefinitionFile { get; set; }

    [CommandSwitch("--manifest-file")]
    public string? ManifestFile { get; set; }

    [CommandSwitch("--manifest-params-file")]
    public string? ManifestParamsFile { get; set; }

    [BooleanCommandSwitch("--no-subscription-permissions")]
    public bool? NoSubscriptionPermissions { get; set; }

    [CommandSwitch("--parameters-file")]
    public string? ParametersFile { get; set; }

    [CommandSwitch("--skip")]
    public string? Skip { get; set; }
}