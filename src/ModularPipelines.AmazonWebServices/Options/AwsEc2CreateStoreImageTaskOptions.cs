using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-store-image-task")]
public record AwsEc2CreateStoreImageTaskOptions(
[property: CliOption("--image-id")] string ImageId,
[property: CliOption("--bucket")] string Bucket
) : AwsOptions
{
    [CliOption("--s3-object-tags")]
    public string[]? S3ObjectTags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}