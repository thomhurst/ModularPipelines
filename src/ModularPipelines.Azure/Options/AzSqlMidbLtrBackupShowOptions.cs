using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "midb", "ltr-backup", "show")]
public record AzSqlMidbLtrBackupShowOptions(
[property: CommandSwitch("--database")] string Database,
[property: CommandSwitch("--managed-instance")] string ManagedInstance,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--backup-id")]
    public string? BackupId { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }
}

