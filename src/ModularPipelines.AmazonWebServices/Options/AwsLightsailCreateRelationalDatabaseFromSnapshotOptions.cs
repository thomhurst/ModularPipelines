using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "create-relational-database-from-snapshot")]
public record AwsLightsailCreateRelationalDatabaseFromSnapshotOptions(
[property: CommandSwitch("--relational-database-name")] string RelationalDatabaseName
) : AwsOptions
{
    [CommandSwitch("--availability-zone")]
    public string? AvailabilityZone { get; set; }

    [CommandSwitch("--relational-database-snapshot-name")]
    public string? RelationalDatabaseSnapshotName { get; set; }

    [CommandSwitch("--relational-database-bundle-id")]
    public string? RelationalDatabaseBundleId { get; set; }

    [CommandSwitch("--source-relational-database-name")]
    public string? SourceRelationalDatabaseName { get; set; }

    [CommandSwitch("--restore-time")]
    public long? RestoreTime { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}