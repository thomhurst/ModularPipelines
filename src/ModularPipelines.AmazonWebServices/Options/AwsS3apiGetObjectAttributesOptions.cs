using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3api", "get-object-attributes")]
public record AwsS3apiGetObjectAttributesOptions(
[property: CommandSwitch("--bucket")] string Bucket,
[property: CommandSwitch("--key")] string Key,
[property: CommandSwitch("--object-attributes")] string[] ObjectAttributes
) : AwsOptions
{
    [CommandSwitch("--version-id")]
    public string? VersionId { get; set; }

    [CommandSwitch("--max-parts")]
    public int? MaxParts { get; set; }

    [CommandSwitch("--part-number-marker")]
    public int? PartNumberMarker { get; set; }

    [CommandSwitch("--sse-customer-algorithm")]
    public string? SseCustomerAlgorithm { get; set; }

    [CommandSwitch("--sse-customer-key")]
    public string? SseCustomerKey { get; set; }

    [CommandSwitch("--sse-customer-key-md5")]
    public string? SseCustomerKeyMd5 { get; set; }

    [CommandSwitch("--request-payer")]
    public string? RequestPayer { get; set; }

    [CommandSwitch("--expected-bucket-owner")]
    public string? ExpectedBucketOwner { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}