using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sqs", "send-message")]
public record AwsSqsSendMessageOptions(
[property: CommandSwitch("--queue-url")] string QueueUrl,
[property: CommandSwitch("--message-body")] string MessageBody
) : AwsOptions
{
    [CommandSwitch("--delay-seconds")]
    public int? DelaySeconds { get; set; }

    [CommandSwitch("--message-attributes")]
    public IEnumerable<KeyValue>? MessageAttributes { get; set; }

    [CommandSwitch("--message-system-attributes")]
    public IEnumerable<KeyValue>? MessageSystemAttributes { get; set; }

    [CommandSwitch("--message-deduplication-id")]
    public string? MessageDeduplicationId { get; set; }

    [CommandSwitch("--message-group-id")]
    public string? MessageGroupId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}