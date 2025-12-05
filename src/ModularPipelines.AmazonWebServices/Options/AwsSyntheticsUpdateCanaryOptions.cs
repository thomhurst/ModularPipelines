using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("synthetics", "update-canary")]
public record AwsSyntheticsUpdateCanaryOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--code")]
    public string? Code { get; set; }

    [CliOption("--execution-role-arn")]
    public string? ExecutionRoleArn { get; set; }

    [CliOption("--runtime-version")]
    public string? RuntimeVersion { get; set; }

    [CliOption("--schedule")]
    public string? Schedule { get; set; }

    [CliOption("--run-config")]
    public string? RunConfig { get; set; }

    [CliOption("--success-retention-period-in-days")]
    public int? SuccessRetentionPeriodInDays { get; set; }

    [CliOption("--failure-retention-period-in-days")]
    public int? FailureRetentionPeriodInDays { get; set; }

    [CliOption("--vpc-config")]
    public string? VpcConfig { get; set; }

    [CliOption("--visual-reference")]
    public string? VisualReference { get; set; }

    [CliOption("--artifact-s3-location")]
    public string? ArtifactS3Location { get; set; }

    [CliOption("--artifact-config")]
    public string? ArtifactConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}