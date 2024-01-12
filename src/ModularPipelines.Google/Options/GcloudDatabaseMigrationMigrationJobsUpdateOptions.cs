using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("database-migration", "migration-jobs", "update")]
public record GcloudDatabaseMigrationMigrationJobsUpdateOptions(
[property: PositionalArgument] string MigrationJob,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [BooleanCommandSwitch("--no-async")]
    public bool? NoAsync { get; set; }

    [CommandSwitch("--destination")]
    public string? Destination { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--dump-parallel-level")]
    public string? DumpParallelLevel { get; set; }

    [CommandSwitch("--dump-path")]
    public string? DumpPath { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }

    [CommandSwitch("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [BooleanCommandSwitch("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CommandSwitch("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [CommandSwitch("--peer-vpc")]
    public string? PeerVpc { get; set; }

    [BooleanCommandSwitch("--static-ip")]
    public bool? StaticIp { get; set; }

    [CommandSwitch("--vm")]
    public string? Vm { get; set; }

    [CommandSwitch("--vm-ip")]
    public string? VmIp { get; set; }

    [CommandSwitch("--vm-port")]
    public string? VmPort { get; set; }

    [CommandSwitch("--vpc")]
    public string? Vpc { get; set; }
}