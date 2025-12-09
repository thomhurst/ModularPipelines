using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sns", "publish")]
public record AwsSnsPublishOptions(
[property: CliOption("--message")] string Message
) : AwsOptions
{
    [CliOption("--topic-arn")]
    public string? TopicArn { get; set; }

    [CliOption("--target-arn")]
    public string? TargetArn { get; set; }

    [CliOption("--phone-number")]
    public string? PhoneNumber { get; set; }

    [CliOption("--subject")]
    public string? Subject { get; set; }

    [CliOption("--message-structure")]
    public string? MessageStructure { get; set; }

    [CliOption("--message-attributes")]
    public IEnumerable<KeyValue>? MessageAttributes { get; set; }

    [CliOption("--message-deduplication-id")]
    public string? MessageDeduplicationId { get; set; }

    [CliOption("--message-group-id")]
    public string? MessageGroupId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}