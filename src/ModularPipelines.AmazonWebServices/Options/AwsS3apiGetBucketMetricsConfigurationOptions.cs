using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3api", "get-bucket-metrics-configuration")]
public record AwsS3apiGetBucketMetricsConfigurationOptions(
[property: CliOption("--bucket")] string Bucket,
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--expected-bucket-owner")]
    public string? ExpectedBucketOwner { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}