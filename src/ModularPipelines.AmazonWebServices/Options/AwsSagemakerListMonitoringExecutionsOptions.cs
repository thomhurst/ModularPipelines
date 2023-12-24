using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "list-monitoring-executions")]
public record AwsSagemakerListMonitoringExecutionsOptions : AwsOptions
{
    [CommandSwitch("--monitoring-schedule-name")]
    public string? MonitoringScheduleName { get; set; }

    [CommandSwitch("--endpoint-name")]
    public string? EndpointName { get; set; }

    [CommandSwitch("--sort-by")]
    public string? SortBy { get; set; }

    [CommandSwitch("--sort-order")]
    public string? SortOrder { get; set; }

    [CommandSwitch("--scheduled-time-before")]
    public long? ScheduledTimeBefore { get; set; }

    [CommandSwitch("--scheduled-time-after")]
    public long? ScheduledTimeAfter { get; set; }

    [CommandSwitch("--creation-time-before")]
    public long? CreationTimeBefore { get; set; }

    [CommandSwitch("--creation-time-after")]
    public long? CreationTimeAfter { get; set; }

    [CommandSwitch("--last-modified-time-before")]
    public long? LastModifiedTimeBefore { get; set; }

    [CommandSwitch("--last-modified-time-after")]
    public long? LastModifiedTimeAfter { get; set; }

    [CommandSwitch("--status-equals")]
    public string? StatusEquals { get; set; }

    [CommandSwitch("--monitoring-job-definition-name")]
    public string? MonitoringJobDefinitionName { get; set; }

    [CommandSwitch("--monitoring-type-equals")]
    public string? MonitoringTypeEquals { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}