using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3api", "delete-objects")]
public record AwsS3apiDeleteObjectsOptions(
[property: CommandSwitch("--bucket")] string Bucket,
[property: CommandSwitch("--delete")] string Delete
) : AwsOptions
{
    [CommandSwitch("--mfa")]
    public string? Mfa { get; set; }

    [CommandSwitch("--request-payer")]
    public string? RequestPayer { get; set; }

    [CommandSwitch("--expected-bucket-owner")]
    public string? ExpectedBucketOwner { get; set; }

    [CommandSwitch("--checksum-algorithm")]
    public string? ChecksumAlgorithm { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}