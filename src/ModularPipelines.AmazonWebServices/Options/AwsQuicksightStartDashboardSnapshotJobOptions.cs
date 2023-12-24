using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quicksight", "start-dashboard-snapshot-job")]
public record AwsQuicksightStartDashboardSnapshotJobOptions(
[property: CommandSwitch("--aws-account-id")] string AwsAccountId,
[property: CommandSwitch("--dashboard-id")] string DashboardId,
[property: CommandSwitch("--snapshot-job-id")] string SnapshotJobId,
[property: CommandSwitch("--user-configuration")] string UserConfiguration,
[property: CommandSwitch("--snapshot-configuration")] string SnapshotConfiguration
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}