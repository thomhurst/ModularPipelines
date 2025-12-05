using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "create-relational-database-snapshot")]
public record AwsLightsailCreateRelationalDatabaseSnapshotOptions(
[property: CliOption("--relational-database-name")] string RelationalDatabaseName,
[property: CliOption("--relational-database-snapshot-name")] string RelationalDatabaseSnapshotName
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}