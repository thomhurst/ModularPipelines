using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("netapp", "volumes", "replications", "reverse")]
public record GcloudNetappVolumesReplicationsReverseOptions(
[property: CliArgument] string Replication,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--volume")]
    public string? Volume { get; set; }
}