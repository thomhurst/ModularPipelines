using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch", "update-job-queue")]
public record AwsBatchUpdateJobQueueOptions(
[property: CommandSwitch("--job-queue")] string JobQueue
) : AwsOptions
{
    [CommandSwitch("--state")]
    public string? State { get; set; }

    [CommandSwitch("--scheduling-policy-arn")]
    public string? SchedulingPolicyArn { get; set; }

    [CommandSwitch("--priority")]
    public int? Priority { get; set; }

    [CommandSwitch("--compute-environment-order")]
    public string[]? ComputeEnvironmentOrder { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}