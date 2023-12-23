using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wisdom", "delete-assistant-association")]
public record AwsWisdomDeleteAssistantAssociationOptions(
[property: CommandSwitch("--assistant-association-id")] string AssistantAssociationId,
[property: CommandSwitch("--assistant-id")] string AssistantId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}