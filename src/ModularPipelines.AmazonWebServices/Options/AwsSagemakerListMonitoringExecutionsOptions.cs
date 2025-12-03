using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "list-monitoring-executions")]
public record AwsSagemakerListMonitoringExecutionsOptions : AwsOptions
{
    [CliOption("--monitoring-schedule-name")]
    public string? MonitoringScheduleName { get; set; }

    [CliOption("--endpoint-name")]
    public string? EndpointName { get; set; }

    [CliOption("--sort-by")]
    public string? SortBy { get; set; }

    [CliOption("--sort-order")]
    public string? SortOrder { get; set; }

    [CliOption("--scheduled-time-before")]
    public long? ScheduledTimeBefore { get; set; }

    [CliOption("--scheduled-time-after")]
    public long? ScheduledTimeAfter { get; set; }

    [CliOption("--creation-time-before")]
    public long? CreationTimeBefore { get; set; }

    [CliOption("--creation-time-after")]
    public long? CreationTimeAfter { get; set; }

    [CliOption("--last-modified-time-before")]
    public long? LastModifiedTimeBefore { get; set; }

    [CliOption("--last-modified-time-after")]
    public long? LastModifiedTimeAfter { get; set; }

    [CliOption("--status-equals")]
    public string? StatusEquals { get; set; }

    [CliOption("--monitoring-job-definition-name")]
    public string? MonitoringJobDefinitionName { get; set; }

    [CliOption("--monitoring-type-equals")]
    public string? MonitoringTypeEquals { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}