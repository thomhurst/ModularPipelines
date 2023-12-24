using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3api", "put-bucket-ownership-controls")]
public record AwsS3apiPutBucketOwnershipControlsOptions(
[property: CommandSwitch("--bucket")] string Bucket,
[property: CommandSwitch("--ownership-controls")] string OwnershipControls
) : AwsOptions
{
    [CommandSwitch("--content-md5")]
    public string? ContentMd5 { get; set; }

    [CommandSwitch("--expected-bucket-owner")]
    public string? ExpectedBucketOwner { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}