using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint-sms-voice-v2", "delete-registration-field-value")]
public record AwsPinpointSmsVoiceV2DeleteRegistrationFieldValueOptions(
[property: CommandSwitch("--registration-id")] string RegistrationId,
[property: CommandSwitch("--field-path")] string FieldPath
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}