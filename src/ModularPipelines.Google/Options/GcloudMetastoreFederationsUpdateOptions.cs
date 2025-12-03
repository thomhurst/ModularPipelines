using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("metastore", "federations", "update")]
public record GcloudMetastoreFederationsUpdateOptions(
[property: CliArgument] string Federation,
[property: CliArgument] string Location,
[property: CliOption("--update-backends")] string UpdateBackends,
[property: CliFlag("--clear-backends")] bool ClearBackends,
[property: CliOption("--remove-backends")] string RemoveBackends
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CliFlag("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CliOption("--remove-labels")]
    public string[]? RemoveLabels { get; set; }
}