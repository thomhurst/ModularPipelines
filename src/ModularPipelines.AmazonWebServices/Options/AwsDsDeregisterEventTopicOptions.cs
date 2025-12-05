using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ds", "deregister-event-topic")]
public record AwsDsDeregisterEventTopicOptions(
[property: CliOption("--directory-id")] string DirectoryId,
[property: CliOption("--topic-name")] string TopicName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}