using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-sms-voice-v2", "delete-verified-destination-number")]
public record AwsPinpointSmsVoiceV2DeleteVerifiedDestinationNumberOptions(
[property: CliOption("--verified-destination-number-id")] string VerifiedDestinationNumberId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}