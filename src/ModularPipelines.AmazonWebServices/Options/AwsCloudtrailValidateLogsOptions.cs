using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudtrail", "validate-logs")]
public record AwsCloudtrailValidateLogsOptions(
[property: CliOption("--trail-arn")] string TrailArn,
[property: CliOption("--start-time")] string StartTime
) : AwsOptions
{
    [CliOption("--end-time")]
    public string? EndTime { get; set; }

    [CliOption("--s3-bucket")]
    public string? S3Bucket { get; set; }

    [CliOption("--s3-prefix")]
    public string? S3Prefix { get; set; }

    [CliOption("--account-id")]
    public string? AccountId { get; set; }

    [CliFlag("--verbose")]
    public bool? Verbose { get; set; }
}