using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-voice", "update-sip-rule")]
public record AwsChimeSdkVoiceUpdateSipRuleOptions(
[property: CliOption("--sip-rule-id")] string SipRuleId,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--target-applications")]
    public string[]? TargetApplications { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}