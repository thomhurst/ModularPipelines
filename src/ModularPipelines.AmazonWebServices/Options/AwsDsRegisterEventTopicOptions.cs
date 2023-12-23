using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ds", "register-event-topic")]
public record AwsDsRegisterEventTopicOptions(
[property: CommandSwitch("--directory-id")] string DirectoryId,
[property: CommandSwitch("--topic-name")] string TopicName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}