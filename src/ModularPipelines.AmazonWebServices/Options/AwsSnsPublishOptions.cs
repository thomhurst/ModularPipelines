using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sns", "publish")]
public record AwsSnsPublishOptions(
[property: CommandSwitch("--message")] string Message
) : AwsOptions
{
    [CommandSwitch("--topic-arn")]
    public string? TopicArn { get; set; }

    [CommandSwitch("--target-arn")]
    public string? TargetArn { get; set; }

    [CommandSwitch("--phone-number")]
    public string? PhoneNumber { get; set; }

    [CommandSwitch("--subject")]
    public string? Subject { get; set; }

    [CommandSwitch("--message-structure")]
    public string? MessageStructure { get; set; }

    [CommandSwitch("--message-attributes")]
    public IEnumerable<KeyValue>? MessageAttributes { get; set; }

    [CommandSwitch("--message-deduplication-id")]
    public string? MessageDeduplicationId { get; set; }

    [CommandSwitch("--message-group-id")]
    public string? MessageGroupId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}