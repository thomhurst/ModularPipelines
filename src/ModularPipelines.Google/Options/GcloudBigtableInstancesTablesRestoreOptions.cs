using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bigtable", "instances", "tables", "restore")]
public record GcloudBigtableInstancesTablesRestoreOptions(
[property: CommandSwitch("--destination")] string Destination,
[property: CommandSwitch("--destination-instance")] string DestinationInstance,
[property: CommandSwitch("--source")] string Source,
[property: CommandSwitch("--source-cluster")] string SourceCluster,
[property: CommandSwitch("--source-instance")] string SourceInstance
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}