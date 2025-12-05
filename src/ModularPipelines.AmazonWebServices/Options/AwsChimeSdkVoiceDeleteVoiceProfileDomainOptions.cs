using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-voice", "delete-voice-profile-domain")]
public record AwsChimeSdkVoiceDeleteVoiceProfileDomainOptions(
[property: CliOption("--voice-profile-domain-id")] string VoiceProfileDomainId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}