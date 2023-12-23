using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-voice", "update-sip-rule")]
public record AwsChimeSdkVoiceUpdateSipRuleOptions(
[property: CommandSwitch("--sip-rule-id")] string SipRuleId,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--target-applications")]
    public string[]? TargetApplications { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}