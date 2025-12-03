using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("database-migration", "migration-jobs", "update")]
public record GcloudDatabaseMigrationMigrationJobsUpdateOptions(
[property: CliArgument] string MigrationJob,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliFlag("--no-async")]
    public bool? NoAsync { get; set; }

    [CliOption("--destination")]
    public string? Destination { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--dump-parallel-level")]
    public string? DumpParallelLevel { get; set; }

    [CliOption("--dump-path")]
    public string? DumpPath { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CliFlag("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CliOption("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [CliOption("--peer-vpc")]
    public string? PeerVpc { get; set; }

    [CliFlag("--static-ip")]
    public bool? StaticIp { get; set; }

    [CliOption("--vm")]
    public string? Vm { get; set; }

    [CliOption("--vm-ip")]
    public string? VmIp { get; set; }

    [CliOption("--vm-port")]
    public string? VmPort { get; set; }

    [CliOption("--vpc")]
    public string? Vpc { get; set; }
}