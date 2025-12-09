using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot-jobs-data", "start-next-pending-job-execution")]
public record AwsIotJobsDataStartNextPendingJobExecutionOptions(
[property: CliOption("--thing-name")] string ThingName
) : AwsOptions
{
    [CliOption("--status-details")]
    public IEnumerable<KeyValue>? StatusDetails { get; set; }

    [CliOption("--step-timeout-in-minutes")]
    public long? StepTimeoutInMinutes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}