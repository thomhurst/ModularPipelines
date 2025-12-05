using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "create-ota-update")]
public record AwsIotCreateOtaUpdateOptions(
[property: CliOption("--ota-update-id")] string OtaUpdateId,
[property: CliOption("--targets")] string[] Targets,
[property: CliOption("--files")] string[] Files,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--protocols")]
    public string[]? Protocols { get; set; }

    [CliOption("--target-selection")]
    public string? TargetSelection { get; set; }

    [CliOption("--aws-job-executions-rollout-config")]
    public string? AwsJobExecutionsRolloutConfig { get; set; }

    [CliOption("--aws-job-presigned-url-config")]
    public string? AwsJobPresignedUrlConfig { get; set; }

    [CliOption("--aws-job-abort-config")]
    public string? AwsJobAbortConfig { get; set; }

    [CliOption("--aws-job-timeout-config")]
    public string? AwsJobTimeoutConfig { get; set; }

    [CliOption("--additional-parameters")]
    public IEnumerable<KeyValue>? AdditionalParameters { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}