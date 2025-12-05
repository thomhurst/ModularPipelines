using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3api", "put-bucket-analytics-configuration")]
public record AwsS3apiPutBucketAnalyticsConfigurationOptions(
[property: CliOption("--bucket")] string Bucket,
[property: CliOption("--id")] string Id,
[property: CliOption("--analytics-configuration")] string AnalyticsConfiguration
) : AwsOptions
{
    [CliOption("--expected-bucket-owner")]
    public string? ExpectedBucketOwner { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}