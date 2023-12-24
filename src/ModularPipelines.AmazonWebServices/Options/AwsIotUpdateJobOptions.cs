using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "update-job")]
public record AwsIotUpdateJobOptions(
[property: CommandSwitch("--job-id")] string JobId
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--presigned-url-config")]
    public string? PresignedUrlConfig { get; set; }

    [CommandSwitch("--job-executions-rollout-config")]
    public string? JobExecutionsRolloutConfig { get; set; }

    [CommandSwitch("--abort-config")]
    public string? AbortConfig { get; set; }

    [CommandSwitch("--timeout-config")]
    public string? TimeoutConfig { get; set; }

    [CommandSwitch("--namespace-id")]
    public string? NamespaceId { get; set; }

    [CommandSwitch("--job-executions-retry-config")]
    public string? JobExecutionsRetryConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}