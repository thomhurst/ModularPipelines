using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "du", "update", "init", "v5")]
public record AzIotDuUpdateInitV5Options(
[property: CommandSwitch("--compat")] string Compat,
[property: CommandSwitch("--step")] string Step,
[property: CommandSwitch("--update-name")] string UpdateName,
[property: CommandSwitch("--update-provider")] string UpdateProvider,
[property: CommandSwitch("--update-version")] string UpdateVersion
) : AzOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--file")]
    public string? File { get; set; }

    [BooleanCommandSwitch("--is-deployable")]
    public bool? IsDeployable { get; set; }

    [BooleanCommandSwitch("--no-validation")]
    public bool? NoValidation { get; set; }

    [CommandSwitch("--related-file")]
    public string? RelatedFile { get; set; }
}