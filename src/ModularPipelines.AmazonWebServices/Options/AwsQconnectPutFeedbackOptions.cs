using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("qconnect", "put-feedback")]
public record AwsQconnectPutFeedbackOptions(
[property: CommandSwitch("--assistant-id")] string AssistantId,
[property: CommandSwitch("--content-feedback")] string ContentFeedback,
[property: CommandSwitch("--target-id")] string TargetId,
[property: CommandSwitch("--target-type")] string TargetType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}