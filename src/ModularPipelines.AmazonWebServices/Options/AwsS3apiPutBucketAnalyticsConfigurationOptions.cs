using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3api", "put-bucket-analytics-configuration")]
public record AwsS3apiPutBucketAnalyticsConfigurationOptions(
[property: CommandSwitch("--bucket")] string Bucket,
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--analytics-configuration")] string AnalyticsConfiguration
) : AwsOptions
{
    [CommandSwitch("--expected-bucket-owner")]
    public string? ExpectedBucketOwner { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}