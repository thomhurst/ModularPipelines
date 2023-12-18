using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("postgres", "flexible-server", "migration", "create")]
public record AzPostgresFlexibleServerMigrationCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--properties")] string Properties,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--migration-mode")]
    public string? MigrationMode { get; set; }

    [CommandSwitch("--migration-name")]
    public string? MigrationName { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}

