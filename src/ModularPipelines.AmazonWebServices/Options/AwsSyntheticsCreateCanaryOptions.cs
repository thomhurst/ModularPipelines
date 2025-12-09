using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("synthetics", "create-canary")]
public record AwsSyntheticsCreateCanaryOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--code")] string Code,
[property: CliOption("--artifact-s3-location")] string ArtifactS3Location,
[property: CliOption("--execution-role-arn")] string ExecutionRoleArn,
[property: CliOption("--schedule")] string Schedule,
[property: CliOption("--runtime-version")] string RuntimeVersion
) : AwsOptions
{
    [CliOption("--run-config")]
    public string? RunConfig { get; set; }

    [CliOption("--success-retention-period-in-days")]
    public int? SuccessRetentionPeriodInDays { get; set; }

    [CliOption("--failure-retention-period-in-days")]
    public int? FailureRetentionPeriodInDays { get; set; }

    [CliOption("--vpc-config")]
    public string? VpcConfig { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--artifact-config")]
    public string? ArtifactConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}