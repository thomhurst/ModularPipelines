using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch", "create-job-queue")]
public record AwsBatchCreateJobQueueOptions(
[property: CommandSwitch("--job-queue-name")] string JobQueueName,
[property: CommandSwitch("--priority")] int Priority,
[property: CommandSwitch("--compute-environment-order")] string[] ComputeEnvironmentOrder
) : AwsOptions
{
    [CommandSwitch("--state")]
    public string? State { get; set; }

    [CommandSwitch("--scheduling-policy-arn")]
    public string? SchedulingPolicyArn { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}