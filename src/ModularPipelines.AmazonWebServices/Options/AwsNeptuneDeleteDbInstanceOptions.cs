using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("neptune", "delete-db-instance")]
public record AwsNeptuneDeleteDbInstanceOptions(
[property: CliOption("--db-instance-identifier")] string DbInstanceIdentifier
) : AwsOptions
{
    [CliOption("--final-db-snapshot-identifier")]
    public string? FinalDbSnapshotIdentifier { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}