using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot-jobs-data", "start-next-pending-job-execution")]
public record AwsIotJobsDataStartNextPendingJobExecutionOptions(
[property: CommandSwitch("--thing-name")] string ThingName
) : AwsOptions
{
    [CommandSwitch("--status-details")]
    public IEnumerable<KeyValue>? StatusDetails { get; set; }

    [CommandSwitch("--step-timeout-in-minutes")]
    public long? StepTimeoutInMinutes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}