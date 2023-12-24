using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint-sms-voice-v2", "delete-registration-attachment")]
public record AwsPinpointSmsVoiceV2DeleteRegistrationAttachmentOptions(
[property: CommandSwitch("--registration-attachment-id")] string RegistrationAttachmentId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}