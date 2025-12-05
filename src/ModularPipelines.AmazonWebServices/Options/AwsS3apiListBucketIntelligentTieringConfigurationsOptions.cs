using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3api", "list-bucket-intelligent-tiering-configurations")]
public record AwsS3apiListBucketIntelligentTieringConfigurationsOptions(
[property: CliOption("--bucket")] string Bucket
) : AwsOptions
{
    [CliOption("--continuation-token")]
    public string? ContinuationToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}