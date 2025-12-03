using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "update-refresh-schedule")]
public record AwsQuicksightUpdateRefreshScheduleOptions(
[property: CliOption("--data-set-id")] string DataSetId,
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--schedule")] string Schedule
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}