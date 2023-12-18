using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datamigration", "tde-migration")]
public record AzDatamigrationTdeMigrationOptions : AzOptions
{
    [CommandSwitch("--database-name")]
    public string? DatabaseName { get; set; }

    [CommandSwitch("--network-share-domain")]
    public string? NetworkShareDomain { get; set; }

    [CommandSwitch("--network-share-password")]
    public string? NetworkSharePassword { get; set; }

    [CommandSwitch("--network-share-path")]
    public string? NetworkSharePath { get; set; }

    [CommandSwitch("--network-share-user-name")]
    public string? NetworkShareUserName { get; set; }

    [CommandSwitch("--source-sql-connection-string")]
    public string? SourceSqlConnectionString { get; set; }

    [CommandSwitch("--target-managed-instance-name")]
    public string? TargetManagedInstanceName { get; set; }

    [CommandSwitch("--target-resource-group-name")]
    public string? TargetResourceGroupName { get; set; }

    [CommandSwitch("--target-subscription-id")]
    public string? TargetSubscriptionId { get; set; }
}