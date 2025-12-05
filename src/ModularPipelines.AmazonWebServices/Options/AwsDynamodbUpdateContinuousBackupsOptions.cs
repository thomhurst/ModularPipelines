using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dynamodb", "update-continuous-backups")]
public record AwsDynamodbUpdateContinuousBackupsOptions(
[property: CliOption("--table-name")] string TableName,
[property: CliOption("--point-in-time-recovery-specification")] string PointInTimeRecoverySpecification
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}