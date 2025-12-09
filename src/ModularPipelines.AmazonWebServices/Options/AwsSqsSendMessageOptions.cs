using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sqs", "send-message")]
public record AwsSqsSendMessageOptions(
[property: CliOption("--queue-url")] string QueueUrl,
[property: CliOption("--message-body")] string MessageBody
) : AwsOptions
{
    [CliOption("--delay-seconds")]
    public int? DelaySeconds { get; set; }

    [CliOption("--message-attributes")]
    public IEnumerable<KeyValue>? MessageAttributes { get; set; }

    [CliOption("--message-system-attributes")]
    public IEnumerable<KeyValue>? MessageSystemAttributes { get; set; }

    [CliOption("--message-deduplication-id")]
    public string? MessageDeduplicationId { get; set; }

    [CliOption("--message-group-id")]
    public string? MessageGroupId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}