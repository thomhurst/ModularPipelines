using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("datamigration", "tde-migration")]
public record AzDatamigrationTdeMigrationOptions : AzOptions
{
    [CliOption("--database-name")]
    public string? DatabaseName { get; set; }

    [CliOption("--network-share-domain")]
    public string? NetworkShareDomain { get; set; }

    [CliOption("--network-share-password")]
    public string? NetworkSharePassword { get; set; }

    [CliOption("--network-share-path")]
    public string? NetworkSharePath { get; set; }

    [CliOption("--network-share-user-name")]
    public string? NetworkShareUserName { get; set; }

    [CliOption("--source-sql-connection-string")]
    public string? SourceSqlConnectionString { get; set; }

    [CliOption("--target-managed-instance-name")]
    public string? TargetManagedInstanceName { get; set; }

    [CliOption("--target-resource-group-name")]
    public string? TargetResourceGroupName { get; set; }

    [CliOption("--target-subscription-id")]
    public string? TargetSubscriptionId { get; set; }
}