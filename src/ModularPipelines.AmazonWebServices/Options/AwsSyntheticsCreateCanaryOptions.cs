using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synthetics", "create-canary")]
public record AwsSyntheticsCreateCanaryOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--code")] string Code,
[property: CommandSwitch("--artifact-s3-location")] string ArtifactS3Location,
[property: CommandSwitch("--execution-role-arn")] string ExecutionRoleArn,
[property: CommandSwitch("--schedule")] string Schedule,
[property: CommandSwitch("--runtime-version")] string RuntimeVersion
) : AwsOptions
{
    [CommandSwitch("--run-config")]
    public string? RunConfig { get; set; }

    [CommandSwitch("--success-retention-period-in-days")]
    public int? SuccessRetentionPeriodInDays { get; set; }

    [CommandSwitch("--failure-retention-period-in-days")]
    public int? FailureRetentionPeriodInDays { get; set; }

    [CommandSwitch("--vpc-config")]
    public string? VpcConfig { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--artifact-config")]
    public string? ArtifactConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}