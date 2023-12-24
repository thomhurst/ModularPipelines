using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sqs", "receive-message")]
public record AwsSqsReceiveMessageOptions(
[property: CommandSwitch("--queue-url")] string QueueUrl
) : AwsOptions
{
    [CommandSwitch("--attribute-names")]
    public string[]? AttributeNames { get; set; }

    [CommandSwitch("--message-attribute-names")]
    public string[]? MessageAttributeNames { get; set; }

    [CommandSwitch("--max-number-of-messages")]
    public int? MaxNumberOfMessages { get; set; }

    [CommandSwitch("--visibility-timeout")]
    public int? VisibilityTimeout { get; set; }

    [CommandSwitch("--wait-time-seconds")]
    public int? WaitTimeSeconds { get; set; }

    [CommandSwitch("--receive-request-attempt-id")]
    public string? ReceiveRequestAttemptId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}