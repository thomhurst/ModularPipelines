using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mgn", "start-export")]
public record AwsMgnStartExportOptions(
[property: CommandSwitch("--s3-bucket")] string S3Bucket,
[property: CommandSwitch("--s3-key")] string S3Key
) : AwsOptions
{
    [CommandSwitch("--s3-bucket-owner")]
    public string? S3BucketOwner { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}