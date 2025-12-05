using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("composer", "environments", "snapshots", "save")]
public record GcloudComposerEnvironmentsSnapshotsSaveOptions(
[property: CliArgument] string Environment,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--snapshot-location")]
    public string? SnapshotLocation { get; set; }
}