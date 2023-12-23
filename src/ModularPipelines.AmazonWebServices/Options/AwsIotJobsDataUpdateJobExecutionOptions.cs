using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot-jobs-data", "update-job-execution")]
public record AwsIotJobsDataUpdateJobExecutionOptions(
[property: CommandSwitch("--job-id")] string JobId,
[property: CommandSwitch("--thing-name")] string ThingName,
[property: CommandSwitch("--status")] string Status
) : AwsOptions
{
    [CommandSwitch("--status-details")]
    public IEnumerable<KeyValue>? StatusDetails { get; set; }

    [CommandSwitch("--step-timeout-in-minutes")]
    public long? StepTimeoutInMinutes { get; set; }

    [CommandSwitch("--expected-version")]
    public long? ExpectedVersion { get; set; }

    [CommandSwitch("--execution-number")]
    public long? ExecutionNumber { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}