using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("synapse", "sql-script", "create")]
public record AzSynapseSqlScriptCreateOptions(
[property: CliOption("--file")] string File,
[property: CliOption("--name")] string Name,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--additional-properties")]
    public string? AdditionalProperties { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--folder-name")]
    public string? FolderName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--result-limit")]
    public string? ResultLimit { get; set; }

    [CliOption("--sql-database-name")]
    public string? SqlDatabaseName { get; set; }

    [CliOption("--sql-pool-name")]
    public string? SqlPoolName { get; set; }
}