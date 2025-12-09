using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sqs", "change-message-visibility-batch")]
public record AwsSqsChangeMessageVisibilityBatchOptions(
[property: CliOption("--queue-url")] string QueueUrl,
[property: CliOption("--entries")] string[] Entries
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}