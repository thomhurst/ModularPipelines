using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sqs", "delete-message-batch")]
public record AwsSqsDeleteMessageBatchOptions(
[property: CliOption("--queue-url")] string QueueUrl,
[property: CliOption("--entries")] string[] Entries
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}