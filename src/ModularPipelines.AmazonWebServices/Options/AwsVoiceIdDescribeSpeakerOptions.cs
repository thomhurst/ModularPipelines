using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("voice-id", "describe-speaker")]
public record AwsVoiceIdDescribeSpeakerOptions(
[property: CliOption("--domain-id")] string DomainId,
[property: CliOption("--speaker-id")] string SpeakerId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}