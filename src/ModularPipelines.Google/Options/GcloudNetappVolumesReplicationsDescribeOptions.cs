using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("netapp", "volumes", "replications", "describe")]
public record GcloudNetappVolumesReplicationsDescribeOptions(
[property: CliArgument] string Replication,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--volume")]
    public string? Volume { get; set; }
}