using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sqs", "get-queue-attributes")]
public record AwsSqsGetQueueAttributesOptions(
[property: CliOption("--queue-url")] string QueueUrl
) : AwsOptions
{
    [CliOption("--attribute-names")]
    public string[]? AttributeNames { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}