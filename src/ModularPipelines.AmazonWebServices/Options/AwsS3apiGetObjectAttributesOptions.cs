using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3api", "get-object-attributes")]
public record AwsS3apiGetObjectAttributesOptions(
[property: CliOption("--bucket")] string Bucket,
[property: CliOption("--key")] string Key,
[property: CliOption("--object-attributes")] string[] ObjectAttributes
) : AwsOptions
{
    [CliOption("--version-id")]
    public string? VersionId { get; set; }

    [CliOption("--max-parts")]
    public int? MaxParts { get; set; }

    [CliOption("--part-number-marker")]
    public int? PartNumberMarker { get; set; }

    [CliOption("--sse-customer-algorithm")]
    public string? SseCustomerAlgorithm { get; set; }

    [CliOption("--sse-customer-key")]
    public string? SseCustomerKey { get; set; }

    [CliOption("--sse-customer-key-md5")]
    public string? SseCustomerKeyMd5 { get; set; }

    [CliOption("--request-payer")]
    public string? RequestPayer { get; set; }

    [CliOption("--expected-bucket-owner")]
    public string? ExpectedBucketOwner { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}