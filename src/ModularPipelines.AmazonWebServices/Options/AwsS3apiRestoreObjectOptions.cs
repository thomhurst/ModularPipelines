using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3api", "restore-object")]
public record AwsS3apiRestoreObjectOptions(
[property: CommandSwitch("--bucket")] string Bucket,
[property: CommandSwitch("--key")] string Key
) : AwsOptions
{
    [CommandSwitch("--version-id")]
    public string? VersionId { get; set; }

    [CommandSwitch("--restore-request")]
    public string? RestoreRequest { get; set; }

    [CommandSwitch("--request-payer")]
    public string? RequestPayer { get; set; }

    [CommandSwitch("--checksum-algorithm")]
    public string? ChecksumAlgorithm { get; set; }

    [CommandSwitch("--expected-bucket-owner")]
    public string? ExpectedBucketOwner { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}