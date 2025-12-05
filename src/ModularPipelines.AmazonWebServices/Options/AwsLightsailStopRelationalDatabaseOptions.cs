using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "stop-relational-database")]
public record AwsLightsailStopRelationalDatabaseOptions(
[property: CliOption("--relational-database-name")] string RelationalDatabaseName
) : AwsOptions
{
    [CliOption("--relational-database-snapshot-name")]
    public string? RelationalDatabaseSnapshotName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}