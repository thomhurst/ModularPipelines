using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bigtable", "instances", "tables", "restore")]
public record GcloudBigtableInstancesTablesRestoreOptions(
[property: CliOption("--destination")] string Destination,
[property: CliOption("--destination-instance")] string DestinationInstance,
[property: CliOption("--source")] string Source,
[property: CliOption("--source-cluster")] string SourceCluster,
[property: CliOption("--source-instance")] string SourceInstance
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}