using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3api", "list-parts")]
public record AwsS3apiListPartsOptions(
[property: CommandSwitch("--bucket")] string Bucket,
[property: CommandSwitch("--key")] string Key,
[property: CommandSwitch("--upload-id")] string UploadId
) : AwsOptions
{
    [CommandSwitch("--request-payer")]
    public string? RequestPayer { get; set; }

    [CommandSwitch("--expected-bucket-owner")]
    public string? ExpectedBucketOwner { get; set; }

    [CommandSwitch("--sse-customer-algorithm")]
    public string? SseCustomerAlgorithm { get; set; }

    [CommandSwitch("--sse-customer-key")]
    public string? SseCustomerKey { get; set; }

    [CommandSwitch("--sse-customer-key-md5")]
    public string? SseCustomerKeyMd5 { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}