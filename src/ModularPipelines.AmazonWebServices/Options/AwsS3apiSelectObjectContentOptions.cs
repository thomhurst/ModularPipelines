using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3api", "select-object-content")]
public record AwsS3apiSelectObjectContentOptions(
[property: CliOption("--bucket")] string Bucket,
[property: CliOption("--key")] string Key,
[property: CliOption("--expression")] string Expression,
[property: CliOption("--expression-type")] string ExpressionType,
[property: CliOption("--input-serialization")] string InputSerialization,
[property: CliOption("--output-serialization")] string OutputSerialization
) : AwsOptions
{
    [CliOption("--sse-customer-algorithm")]
    public string? SseCustomerAlgorithm { get; set; }

    [CliOption("--sse-customer-key")]
    public string? SseCustomerKey { get; set; }

    [CliOption("--sse-customer-key-md5")]
    public string? SseCustomerKeyMd5 { get; set; }

    [CliOption("--request-progress")]
    public string? RequestProgress { get; set; }

    [CliOption("--scan-range")]
    public string? ScanRange { get; set; }

    [CliOption("--expected-bucket-owner")]
    public string? ExpectedBucketOwner { get; set; }
}