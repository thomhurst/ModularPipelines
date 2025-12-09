using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "dw", "create")]
public record AzSqlDwCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--server")] string Server
) : AzOptions
{
    [CliOption("--availability-zone")]
    public string? AvailabilityZone { get; set; }

    [CliOption("--backup-storage-redundancy")]
    public string? BackupStorageRedundancy { get; set; }

    [CliOption("--collation")]
    public string? Collation { get; set; }

    [CliOption("--max-size")]
    public string? MaxSize { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--service-level-objective")]
    public string? ServiceLevelObjective { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}