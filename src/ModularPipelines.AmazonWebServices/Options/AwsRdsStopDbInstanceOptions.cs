using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "stop-db-instance")]
public record AwsRdsStopDbInstanceOptions(
[property: CliOption("--db-instance-identifier")] string DbInstanceIdentifier
) : AwsOptions
{
    [CliOption("--db-snapshot-identifier")]
    public string? DbSnapshotIdentifier { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}