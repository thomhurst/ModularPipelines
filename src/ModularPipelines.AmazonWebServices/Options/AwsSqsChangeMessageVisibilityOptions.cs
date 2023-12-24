using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sqs", "change-message-visibility")]
public record AwsSqsChangeMessageVisibilityOptions(
[property: CommandSwitch("--queue-url")] string QueueUrl,
[property: CommandSwitch("--receipt-handle")] string ReceiptHandle,
[property: CommandSwitch("--visibility-timeout")] int VisibilityTimeout
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}