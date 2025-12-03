using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sqs", "get-queue-url")]
public record AwsSqsGetQueueUrlOptions(
[property: CliOption("--queue-name")] string QueueName
) : AwsOptions
{
    [CliOption("--queue-owner-aws-account-id")]
    public string? QueueOwnerAwsAccountId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}