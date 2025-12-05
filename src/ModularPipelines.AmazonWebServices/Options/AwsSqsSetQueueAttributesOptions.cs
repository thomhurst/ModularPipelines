using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sqs", "set-queue-attributes")]
public record AwsSqsSetQueueAttributesOptions(
[property: CliOption("--queue-url")] string QueueUrl,
[property: CliOption("--attributes")] IEnumerable<KeyValue> Attributes
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}