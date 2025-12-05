using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3api", "put-bucket-metrics-configuration")]
public record AwsS3apiPutBucketMetricsConfigurationOptions(
[property: CliOption("--bucket")] string Bucket,
[property: CliOption("--id")] string Id,
[property: CliOption("--metrics-configuration")] string MetricsConfiguration
) : AwsOptions
{
    [CliOption("--expected-bucket-owner")]
    public string? ExpectedBucketOwner { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}