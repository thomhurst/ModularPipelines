using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot-jobs-data", "update-job-execution")]
public record AwsIotJobsDataUpdateJobExecutionOptions(
[property: CliOption("--job-id")] string JobId,
[property: CliOption("--thing-name")] string ThingName,
[property: CliOption("--status")] string Status
) : AwsOptions
{
    [CliOption("--status-details")]
    public IEnumerable<KeyValue>? StatusDetails { get; set; }

    [CliOption("--step-timeout-in-minutes")]
    public long? StepTimeoutInMinutes { get; set; }

    [CliOption("--expected-version")]
    public long? ExpectedVersion { get; set; }

    [CliOption("--execution-number")]
    public long? ExecutionNumber { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}