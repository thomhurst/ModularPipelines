using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml-engine", "versions", "update")]
public record GcloudMlEngineVersionsUpdateOptions : GcloudOptions
{
    public GcloudMlEngineVersionsUpdateOptions(
        string version,
        string model
    )
    {
        GcloudMlEngineVersionsUpdateOptionsVersion = version;
        Model = model;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string GcloudMlEngineVersionsUpdateOptionsVersion { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Model { get; set; }

    [CliOption("--config")]
    public string? Config { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CliFlag("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CliOption("--remove-labels")]
    public string[]? RemoveLabels { get; set; }
}
