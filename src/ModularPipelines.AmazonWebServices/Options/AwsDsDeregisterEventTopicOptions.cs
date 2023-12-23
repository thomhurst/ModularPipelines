using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ds", "deregister-event-topic")]
public record AwsDsDeregisterEventTopicOptions(
[property: CommandSwitch("--directory-id")] string DirectoryId,
[property: CommandSwitch("--topic-name")] string TopicName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}