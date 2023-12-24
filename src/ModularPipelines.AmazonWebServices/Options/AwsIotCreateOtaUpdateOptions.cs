using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "create-ota-update")]
public record AwsIotCreateOtaUpdateOptions(
[property: CommandSwitch("--ota-update-id")] string OtaUpdateId,
[property: CommandSwitch("--targets")] string[] Targets,
[property: CommandSwitch("--files")] string[] Files,
[property: CommandSwitch("--role-arn")] string RoleArn
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--protocols")]
    public string[]? Protocols { get; set; }

    [CommandSwitch("--target-selection")]
    public string? TargetSelection { get; set; }

    [CommandSwitch("--aws-job-executions-rollout-config")]
    public string? AwsJobExecutionsRolloutConfig { get; set; }

    [CommandSwitch("--aws-job-presigned-url-config")]
    public string? AwsJobPresignedUrlConfig { get; set; }

    [CommandSwitch("--aws-job-abort-config")]
    public string? AwsJobAbortConfig { get; set; }

    [CommandSwitch("--aws-job-timeout-config")]
    public string? AwsJobTimeoutConfig { get; set; }

    [CommandSwitch("--additional-parameters")]
    public IEnumerable<KeyValue>? AdditionalParameters { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}