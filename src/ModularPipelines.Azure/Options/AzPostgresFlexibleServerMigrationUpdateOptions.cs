using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("postgres", "flexible-server", "migration", "update")]
public record AzPostgresFlexibleServerMigrationUpdateOptions(
[property: CommandSwitch("--migration-name")] string MigrationName
) : AzOptions
{
    [CommandSwitch("--cancel")]
    public string? Cancel { get; set; }

    [CommandSwitch("--cutover")]
    public string? Cutover { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--setup-replication")]
    public string? SetupReplication { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}