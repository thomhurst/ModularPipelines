using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("netapp", "volumes", "revert")]
public record GcloudNetappVolumesRevertOptions(
[property: CliArgument] string Volume,
[property: CliArgument] string Location,
[property: CliOption("--snapshot")] string Snapshot
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}