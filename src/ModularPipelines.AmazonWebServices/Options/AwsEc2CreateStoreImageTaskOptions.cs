using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "create-store-image-task")]
public record AwsEc2CreateStoreImageTaskOptions(
[property: CommandSwitch("--image-id")] string ImageId,
[property: CommandSwitch("--bucket")] string Bucket
) : AwsOptions
{
    [CommandSwitch("--s3-object-tags")]
    public string[]? S3ObjectTags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}