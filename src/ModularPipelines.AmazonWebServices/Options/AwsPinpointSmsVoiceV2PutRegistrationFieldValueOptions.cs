using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint-sms-voice-v2", "put-registration-field-value")]
public record AwsPinpointSmsVoiceV2PutRegistrationFieldValueOptions(
[property: CommandSwitch("--registration-id")] string RegistrationId,
[property: CommandSwitch("--field-path")] string FieldPath
) : AwsOptions
{
    [CommandSwitch("--select-choices")]
    public string[]? SelectChoices { get; set; }

    [CommandSwitch("--text-value")]
    public string? TextValue { get; set; }

    [CommandSwitch("--registration-attachment-id")]
    public string? RegistrationAttachmentId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}