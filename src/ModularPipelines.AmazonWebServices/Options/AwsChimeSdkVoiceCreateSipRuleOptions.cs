using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-voice", "create-sip-rule")]
public record AwsChimeSdkVoiceCreateSipRuleOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--trigger-type")] string TriggerType,
[property: CommandSwitch("--trigger-value")] string TriggerValue
) : AwsOptions
{
    [CommandSwitch("--target-applications")]
    public string[]? TargetApplications { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}