using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-sms-voice-v2", "delete-registration-attachment")]
public record AwsPinpointSmsVoiceV2DeleteRegistrationAttachmentOptions(
[property: CliOption("--registration-attachment-id")] string RegistrationAttachmentId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}