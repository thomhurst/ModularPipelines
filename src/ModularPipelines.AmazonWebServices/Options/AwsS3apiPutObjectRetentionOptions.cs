using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3api", "put-object-retention")]
public record AwsS3apiPutObjectRetentionOptions(
[property: CommandSwitch("--bucket")] string Bucket,
[property: CommandSwitch("--key")] string Key
) : AwsOptions
{
    [CommandSwitch("--retention")]
    public string? Retention { get; set; }

    [CommandSwitch("--request-payer")]
    public string? RequestPayer { get; set; }

    [CommandSwitch("--version-id")]
    public string? VersionId { get; set; }

    [CommandSwitch("--content-md5")]
    public string? ContentMd5 { get; set; }

    [CommandSwitch("--checksum-algorithm")]
    public string? ChecksumAlgorithm { get; set; }

    [CommandSwitch("--expected-bucket-owner")]
    public string? ExpectedBucketOwner { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}