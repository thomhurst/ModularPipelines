using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "get-relational-database-snapshot")]
public record AwsLightsailGetRelationalDatabaseSnapshotOptions(
[property: CliOption("--relational-database-snapshot-name")] string RelationalDatabaseSnapshotName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}