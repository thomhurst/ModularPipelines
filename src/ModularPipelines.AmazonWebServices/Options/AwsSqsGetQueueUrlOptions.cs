using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sqs", "get-queue-url")]
public record AwsSqsGetQueueUrlOptions(
[property: CommandSwitch("--queue-name")] string QueueName
) : AwsOptions
{
    [CommandSwitch("--queue-owner-aws-account-id")]
    public string? QueueOwnerAwsAccountId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}