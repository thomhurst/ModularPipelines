using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "midb", "ltr-backup", "list")]
public record AzSqlMidbLtrBackupListOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions
{
    [CommandSwitch("--database")]
    public string? Database { get; set; }

    [CommandSwitch("--database-state")]
    public string? DatabaseState { get; set; }

    [CommandSwitch("--latest")]
    public string? Latest { get; set; }

    [CommandSwitch("--managed-instance")]
    public string? ManagedInstance { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}

