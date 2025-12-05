using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("batch", "update-job-queue")]
public record AwsBatchUpdateJobQueueOptions(
[property: CliOption("--job-queue")] string JobQueue
) : AwsOptions
{
    [CliOption("--state")]
    public string? State { get; set; }

    [CliOption("--scheduling-policy-arn")]
    public string? SchedulingPolicyArn { get; set; }

    [CliOption("--priority")]
    public int? Priority { get; set; }

    [CliOption("--compute-environment-order")]
    public string[]? ComputeEnvironmentOrder { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}