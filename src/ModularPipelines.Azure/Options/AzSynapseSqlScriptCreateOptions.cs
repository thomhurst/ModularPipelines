using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "sql-script", "create")]
public record AzSynapseSqlScriptCreateOptions(
[property: CommandSwitch("--file")] string File,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CommandSwitch("--additional-properties")]
    public string? AdditionalProperties { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--folder-name")]
    public string? FolderName { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--result-limit")]
    public string? ResultLimit { get; set; }

    [CommandSwitch("--sql-database-name")]
    public string? SqlDatabaseName { get; set; }

    [CommandSwitch("--sql-pool-name")]
    public string? SqlPoolName { get; set; }
}