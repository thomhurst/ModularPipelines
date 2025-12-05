using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-voice", "create-sip-rule")]
public record AwsChimeSdkVoiceCreateSipRuleOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--trigger-type")] string TriggerType,
[property: CliOption("--trigger-value")] string TriggerValue
) : AwsOptions
{
    [CliOption("--target-applications")]
    public string[]? TargetApplications { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}