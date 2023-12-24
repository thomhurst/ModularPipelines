using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("qbusiness", "list-data-source-sync-jobs")]
public record AwsQbusinessListDataSourceSyncJobsOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--data-source-id")] string DataSourceId,
[property: CommandSwitch("--index-id")] string IndexId
) : AwsOptions
{
    [CommandSwitch("--end-time")]
    public long? EndTime { get; set; }

    [CommandSwitch("--start-time")]
    public long? StartTime { get; set; }

    [CommandSwitch("--status-filter")]
    public string? StatusFilter { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}