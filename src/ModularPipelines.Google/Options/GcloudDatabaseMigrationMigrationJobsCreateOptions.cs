using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("database-migration", "migration-jobs", "create")]
public record GcloudDatabaseMigrationMigrationJobsCreateOptions(
[property: PositionalArgument] string MigrationJob,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--destination")] string Destination,
[property: CommandSwitch("--source")] string Source,
[property: CommandSwitch("--type")] string Type
) : GcloudOptions
{
    [BooleanCommandSwitch("--no-async")]
    public bool? NoAsync { get; set; }

    [CommandSwitch("--commit-id")]
    public string? CommitId { get; set; }

    [CommandSwitch("--conversion-workspace")]
    public string? ConversionWorkspace { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--dump-parallel-level")]
    public string? DumpParallelLevel { get; set; }

    [CommandSwitch("--dump-path")]
    public string? DumpPath { get; set; }

    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--cmek-key")]
    public string? CmekKey { get; set; }

    [CommandSwitch("--cmek-keyring")]
    public string? CmekKeyring { get; set; }

    [CommandSwitch("--cmek-project")]
    public string? CmekProject { get; set; }

    [CommandSwitch("--peer-vpc")]
    public string? PeerVpc { get; set; }

    [BooleanCommandSwitch("--static-ip")]
    public bool? StaticIp { get; set; }

    [CommandSwitch("--vm-ip")]
    public string? VmIp { get; set; }

    [CommandSwitch("--vm-port")]
    public string? VmPort { get; set; }

    [CommandSwitch("--vpc")]
    public string? Vpc { get; set; }

    [CommandSwitch("--vm")]
    public string? Vm { get; set; }
}