using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "create-relational-database-from-snapshot")]
public record AwsLightsailCreateRelationalDatabaseFromSnapshotOptions(
[property: CliOption("--relational-database-name")] string RelationalDatabaseName
) : AwsOptions
{
    [CliOption("--availability-zone")]
    public string? AvailabilityZone { get; set; }

    [CliOption("--relational-database-snapshot-name")]
    public string? RelationalDatabaseSnapshotName { get; set; }

    [CliOption("--relational-database-bundle-id")]
    public string? RelationalDatabaseBundleId { get; set; }

    [CliOption("--source-relational-database-name")]
    public string? SourceRelationalDatabaseName { get; set; }

    [CliOption("--restore-time")]
    public long? RestoreTime { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}