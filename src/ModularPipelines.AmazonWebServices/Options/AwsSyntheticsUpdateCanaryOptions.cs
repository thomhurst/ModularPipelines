using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synthetics", "update-canary")]
public record AwsSyntheticsUpdateCanaryOptions(
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--code")]
    public string? Code { get; set; }

    [CommandSwitch("--execution-role-arn")]
    public string? ExecutionRoleArn { get; set; }

    [CommandSwitch("--runtime-version")]
    public string? RuntimeVersion { get; set; }

    [CommandSwitch("--schedule")]
    public string? Schedule { get; set; }

    [CommandSwitch("--run-config")]
    public string? RunConfig { get; set; }

    [CommandSwitch("--success-retention-period-in-days")]
    public int? SuccessRetentionPeriodInDays { get; set; }

    [CommandSwitch("--failure-retention-period-in-days")]
    public int? FailureRetentionPeriodInDays { get; set; }

    [CommandSwitch("--vpc-config")]
    public string? VpcConfig { get; set; }

    [CommandSwitch("--visual-reference")]
    public string? VisualReference { get; set; }

    [CommandSwitch("--artifact-s3-location")]
    public string? ArtifactS3Location { get; set; }

    [CommandSwitch("--artifact-config")]
    public string? ArtifactConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}