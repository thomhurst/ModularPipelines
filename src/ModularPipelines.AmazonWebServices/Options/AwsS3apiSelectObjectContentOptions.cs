using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3api", "select-object-content")]
public record AwsS3apiSelectObjectContentOptions(
[property: CommandSwitch("--bucket")] string Bucket,
[property: CommandSwitch("--key")] string Key,
[property: CommandSwitch("--expression")] string Expression,
[property: CommandSwitch("--expression-type")] string ExpressionType,
[property: CommandSwitch("--input-serialization")] string InputSerialization,
[property: CommandSwitch("--output-serialization")] string OutputSerialization
) : AwsOptions
{
    [CommandSwitch("--sse-customer-algorithm")]
    public string? SseCustomerAlgorithm { get; set; }

    [CommandSwitch("--sse-customer-key")]
    public string? SseCustomerKey { get; set; }

    [CommandSwitch("--sse-customer-key-md5")]
    public string? SseCustomerKeyMd5 { get; set; }

    [CommandSwitch("--request-progress")]
    public string? RequestProgress { get; set; }

    [CommandSwitch("--scan-range")]
    public string? ScanRange { get; set; }

    [CommandSwitch("--expected-bucket-owner")]
    public string? ExpectedBucketOwner { get; set; }
}