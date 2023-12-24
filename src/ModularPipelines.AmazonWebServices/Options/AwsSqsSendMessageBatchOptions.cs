using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sqs", "send-message-batch")]
public record AwsSqsSendMessageBatchOptions(
[property: CommandSwitch("--queue-url")] string QueueUrl,
[property: CommandSwitch("--entries")] string[] Entries
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}