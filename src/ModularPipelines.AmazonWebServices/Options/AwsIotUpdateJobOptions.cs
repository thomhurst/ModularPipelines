using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "update-job")]
public record AwsIotUpdateJobOptions(
[property: CliOption("--job-id")] string JobId
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--presigned-url-config")]
    public string? PresignedUrlConfig { get; set; }

    [CliOption("--job-executions-rollout-config")]
    public string? JobExecutionsRolloutConfig { get; set; }

    [CliOption("--abort-config")]
    public string? AbortConfig { get; set; }

    [CliOption("--timeout-config")]
    public string? TimeoutConfig { get; set; }

    [CliOption("--namespace-id")]
    public string? NamespaceId { get; set; }

    [CliOption("--job-executions-retry-config")]
    public string? JobExecutionsRetryConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}