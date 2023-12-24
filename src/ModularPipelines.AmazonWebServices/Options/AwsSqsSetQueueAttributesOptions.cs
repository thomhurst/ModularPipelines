using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sqs", "set-queue-attributes")]
public record AwsSqsSetQueueAttributesOptions(
[property: CommandSwitch("--queue-url")] string QueueUrl,
[property: CommandSwitch("--attributes")] IEnumerable<KeyValue> Attributes
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}