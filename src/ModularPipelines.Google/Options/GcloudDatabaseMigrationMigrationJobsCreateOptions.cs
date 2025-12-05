using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("database-migration", "migration-jobs", "create")]
public record GcloudDatabaseMigrationMigrationJobsCreateOptions(
[property: CliArgument] string MigrationJob,
[property: CliArgument] string Region,
[property: CliOption("--destination")] string Destination,
[property: CliOption("--source")] string Source,
[property: CliOption("--type")] string Type
) : GcloudOptions
{
    [CliFlag("--no-async")]
    public bool? NoAsync { get; set; }

    [CliOption("--commit-id")]
    public string? CommitId { get; set; }

    [CliOption("--conversion-workspace")]
    public string? ConversionWorkspace { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--dump-parallel-level")]
    public string? DumpParallelLevel { get; set; }

    [CliOption("--dump-path")]
    public string? DumpPath { get; set; }

    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--cmek-key")]
    public string? CmekKey { get; set; }

    [CliOption("--cmek-keyring")]
    public string? CmekKeyring { get; set; }

    [CliOption("--cmek-project")]
    public string? CmekProject { get; set; }

    [CliOption("--peer-vpc")]
    public string? PeerVpc { get; set; }

    [CliFlag("--static-ip")]
    public bool? StaticIp { get; set; }

    [CliOption("--vm-ip")]
    public string? VmIp { get; set; }

    [CliOption("--vm-port")]
    public string? VmPort { get; set; }

    [CliOption("--vpc")]
    public string? Vpc { get; set; }

    [CliOption("--vm")]
    public string? Vm { get; set; }
}