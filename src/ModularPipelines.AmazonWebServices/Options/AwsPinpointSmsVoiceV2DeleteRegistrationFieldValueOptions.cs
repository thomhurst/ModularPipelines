using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-sms-voice-v2", "delete-registration-field-value")]
public record AwsPinpointSmsVoiceV2DeleteRegistrationFieldValueOptions(
[property: CliOption("--registration-id")] string RegistrationId,
[property: CliOption("--field-path")] string FieldPath
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}