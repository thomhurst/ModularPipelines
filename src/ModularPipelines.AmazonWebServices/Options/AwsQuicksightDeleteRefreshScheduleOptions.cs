using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "delete-refresh-schedule")]
public record AwsQuicksightDeleteRefreshScheduleOptions(
[property: CliOption("--data-set-id")] string DataSetId,
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--schedule-id")] string ScheduleId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}