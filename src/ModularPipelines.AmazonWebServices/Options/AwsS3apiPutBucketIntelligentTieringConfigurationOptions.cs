using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3api", "put-bucket-intelligent-tiering-configuration")]
public record AwsS3apiPutBucketIntelligentTieringConfigurationOptions(
[property: CliOption("--bucket")] string Bucket,
[property: CliOption("--id")] string Id,
[property: CliOption("--intelligent-tiering-configuration")] string IntelligentTieringConfiguration
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}