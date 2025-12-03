using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bms", "volumes", "restore")]
public record GcloudBmsVolumesRestoreOptions(
[property: CliArgument] string Volume,
[property: CliArgument] string Region,
[property: CliOption("--snapshot")] string Snapshot
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}