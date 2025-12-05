using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3api", "list-bucket-metrics-configurations")]
public record AwsS3apiListBucketMetricsConfigurationsOptions(
[property: CliOption("--bucket")] string Bucket
) : AwsOptions
{
    [CliOption("--continuation-token")]
    public string? ContinuationToken { get; set; }

    [CliOption("--expected-bucket-owner")]
    public string? ExpectedBucketOwner { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}