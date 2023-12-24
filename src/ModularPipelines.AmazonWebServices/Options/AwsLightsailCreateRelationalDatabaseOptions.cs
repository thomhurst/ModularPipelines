using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "create-relational-database")]
public record AwsLightsailCreateRelationalDatabaseOptions(
[property: CommandSwitch("--relational-database-name")] string RelationalDatabaseName,
[property: CommandSwitch("--relational-database-blueprint-id")] string RelationalDatabaseBlueprintId,
[property: CommandSwitch("--relational-database-bundle-id")] string RelationalDatabaseBundleId,
[property: CommandSwitch("--master-database-name")] string MasterDatabaseName,
[property: CommandSwitch("--master-username")] string MasterUsername
) : AwsOptions
{
    [CommandSwitch("--availability-zone")]
    public string? AvailabilityZone { get; set; }

    [CommandSwitch("--master-user-password")]
    public string? MasterUserPassword { get; set; }

    [CommandSwitch("--preferred-backup-window")]
    public string? PreferredBackupWindow { get; set; }

    [CommandSwitch("--preferred-maintenance-window")]
    public string? PreferredMaintenanceWindow { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}