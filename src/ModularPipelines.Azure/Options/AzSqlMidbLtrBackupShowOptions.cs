using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "midb", "ltr-backup", "show")]
public record AzSqlMidbLtrBackupShowOptions : AzOptions
{
    [CommandSwitch("--backup-id")]
    public string? BackupId { get; set; }

    [CommandSwitch("--database")]
    public string? Database { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--managed-instance")]
    public string? ManagedInstance { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }
}