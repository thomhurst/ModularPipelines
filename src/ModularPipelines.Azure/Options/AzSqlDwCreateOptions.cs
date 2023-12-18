using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "dw", "create")]
public record AzSqlDwCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--server")] string Server
) : AzOptions
{
    [CommandSwitch("--availability-zone")]
    public string? AvailabilityZone { get; set; }

    [CommandSwitch("--backup-storage-redundancy")]
    public string? BackupStorageRedundancy { get; set; }

    [CommandSwitch("--collation")]
    public string? Collation { get; set; }

    [CommandSwitch("--max-size")]
    public string? MaxSize { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--service-level-objective")]
    public string? ServiceLevelObjective { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}