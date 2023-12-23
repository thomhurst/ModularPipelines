using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudtrail", "validate-logs")]
public record AwsCloudtrailValidateLogsOptions(
[property: CommandSwitch("--trail-arn")] string TrailArn,
[property: CommandSwitch("--start-time")] string StartTime
) : AwsOptions
{
    [CommandSwitch("--end-time")]
    public string? EndTime { get; set; }

    [CommandSwitch("--s3-bucket")]
    public string? S3Bucket { get; set; }

    [CommandSwitch("--s3-prefix")]
    public string? S3Prefix { get; set; }

    [CommandSwitch("--account-id")]
    public string? AccountId { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public bool? Verbose { get; set; }
}