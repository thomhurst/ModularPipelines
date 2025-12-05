using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sqs", "change-message-visibility")]
public record AwsSqsChangeMessageVisibilityOptions(
[property: CliOption("--queue-url")] string QueueUrl,
[property: CliOption("--receipt-handle")] string ReceiptHandle,
[property: CliOption("--visibility-timeout")] int VisibilityTimeout
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}