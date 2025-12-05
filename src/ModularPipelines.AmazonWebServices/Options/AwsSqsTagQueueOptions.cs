using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sqs", "tag-queue")]
public record AwsSqsTagQueueOptions(
[property: CliOption("--queue-url")] string QueueUrl,
[property: CliOption("--tags")] IEnumerable<KeyValue> Tags
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}