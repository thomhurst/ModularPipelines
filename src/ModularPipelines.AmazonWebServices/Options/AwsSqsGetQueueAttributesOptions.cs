using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sqs", "get-queue-attributes")]
public record AwsSqsGetQueueAttributesOptions(
[property: CommandSwitch("--queue-url")] string QueueUrl
) : AwsOptions
{
    [CommandSwitch("--attribute-names")]
    public string[]? AttributeNames { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}