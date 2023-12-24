using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sqs", "delete-message")]
public record AwsSqsDeleteMessageOptions(
[property: CommandSwitch("--queue-url")] string QueueUrl,
[property: CommandSwitch("--receipt-handle")] string ReceiptHandle
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}