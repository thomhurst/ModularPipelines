using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "delete-bucket-access-key")]
public record AwsLightsailDeleteBucketAccessKeyOptions(
[property: CliOption("--bucket-name")] string BucketName,
[property: CliOption("--access-key-id")] string AccessKeyId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}