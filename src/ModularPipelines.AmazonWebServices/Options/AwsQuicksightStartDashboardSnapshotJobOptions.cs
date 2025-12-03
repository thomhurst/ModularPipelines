using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "start-dashboard-snapshot-job")]
public record AwsQuicksightStartDashboardSnapshotJobOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--dashboard-id")] string DashboardId,
[property: CliOption("--snapshot-job-id")] string SnapshotJobId,
[property: CliOption("--user-configuration")] string UserConfiguration,
[property: CliOption("--snapshot-configuration")] string SnapshotConfiguration
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}