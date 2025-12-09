using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ds", "register-event-topic")]
public record AwsDsRegisterEventTopicOptions(
[property: CliOption("--directory-id")] string DirectoryId,
[property: CliOption("--topic-name")] string TopicName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}