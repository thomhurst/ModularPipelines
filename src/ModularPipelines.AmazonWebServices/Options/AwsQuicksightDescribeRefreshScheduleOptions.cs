using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "describe-refresh-schedule")]
public record AwsQuicksightDescribeRefreshScheduleOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--data-set-id")] string DataSetId,
[property: CliOption("--schedule-id")] string ScheduleId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}