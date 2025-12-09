using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("batch", "create-job-queue")]
public record AwsBatchCreateJobQueueOptions(
[property: CliOption("--job-queue-name")] string JobQueueName,
[property: CliOption("--priority")] int Priority,
[property: CliOption("--compute-environment-order")] string[] ComputeEnvironmentOrder
) : AwsOptions
{
    [CliOption("--state")]
    public string? State { get; set; }

    [CliOption("--scheduling-policy-arn")]
    public string? SchedulingPolicyArn { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}