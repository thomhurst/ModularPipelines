using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint-sms-voice-v2", "delete-verified-destination-number")]
public record AwsPinpointSmsVoiceV2DeleteVerifiedDestinationNumberOptions(
[property: CommandSwitch("--verified-destination-number-id")] string VerifiedDestinationNumberId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}