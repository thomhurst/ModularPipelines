using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sqs", "receive-message")]
public record AwsSqsReceiveMessageOptions(
[property: CliOption("--queue-url")] string QueueUrl
) : AwsOptions
{
    [CliOption("--attribute-names")]
    public string[]? AttributeNames { get; set; }

    [CliOption("--message-attribute-names")]
    public string[]? MessageAttributeNames { get; set; }

    [CliOption("--max-number-of-messages")]
    public int? MaxNumberOfMessages { get; set; }

    [CliOption("--visibility-timeout")]
    public int? VisibilityTimeout { get; set; }

    [CliOption("--wait-time-seconds")]
    public int? WaitTimeSeconds { get; set; }

    [CliOption("--receive-request-attempt-id")]
    public string? ReceiveRequestAttemptId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}