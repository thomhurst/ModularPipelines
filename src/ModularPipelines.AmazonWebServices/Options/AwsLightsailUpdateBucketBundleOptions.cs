using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "update-bucket-bundle")]
public record AwsLightsailUpdateBucketBundleOptions(
[property: CliOption("--bucket-name")] string BucketName,
[property: CliOption("--bundle-id")] string BundleId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}