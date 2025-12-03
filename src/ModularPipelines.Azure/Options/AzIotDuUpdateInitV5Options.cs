using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "du", "update", "init", "v5")]
public record AzIotDuUpdateInitV5Options(
[property: CliOption("--compat")] string Compat,
[property: CliOption("--step")] string Step,
[property: CliOption("--update-name")] string UpdateName,
[property: CliOption("--update-provider")] string UpdateProvider,
[property: CliOption("--update-version")] string UpdateVersion
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--file")]
    public string? File { get; set; }

    [CliFlag("--is-deployable")]
    public bool? IsDeployable { get; set; }

    [CliFlag("--no-validation")]
    public bool? NoValidation { get; set; }

    [CliOption("--related-file")]
    public string? RelatedFile { get; set; }
}