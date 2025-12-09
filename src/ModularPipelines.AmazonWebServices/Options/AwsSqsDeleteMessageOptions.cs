using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sqs", "delete-message")]
public record AwsSqsDeleteMessageOptions(
[property: CliOption("--queue-url")] string QueueUrl,
[property: CliOption("--receipt-handle")] string ReceiptHandle
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}