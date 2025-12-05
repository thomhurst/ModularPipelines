using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "create-relational-database")]
public record AwsLightsailCreateRelationalDatabaseOptions(
[property: CliOption("--relational-database-name")] string RelationalDatabaseName,
[property: CliOption("--relational-database-blueprint-id")] string RelationalDatabaseBlueprintId,
[property: CliOption("--relational-database-bundle-id")] string RelationalDatabaseBundleId,
[property: CliOption("--master-database-name")] string MasterDatabaseName,
[property: CliOption("--master-username")] string MasterUsername
) : AwsOptions
{
    [CliOption("--availability-zone")]
    public string? AvailabilityZone { get; set; }

    [CliOption("--master-user-password")]
    public string? MasterUserPassword { get; set; }

    [CliOption("--preferred-backup-window")]
    public string? PreferredBackupWindow { get; set; }

    [CliOption("--preferred-maintenance-window")]
    public string? PreferredMaintenanceWindow { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}