using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("voice-id", "evaluate-session")]
public record AwsVoiceIdEvaluateSessionOptions(
[property: CommandSwitch("--domain-id")] string DomainId,
[property: CommandSwitch("--session-name-or-id")] string SessionNameOrId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}