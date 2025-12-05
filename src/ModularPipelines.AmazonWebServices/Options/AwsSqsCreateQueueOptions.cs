using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sqs", "create-queue")]
public record AwsSqsCreateQueueOptions(
[property: CliOption("--queue-name")] string QueueName
) : AwsOptions
{
    [CliOption("--attributes")]
    public IEnumerable<KeyValue>? Attributes { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}