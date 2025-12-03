using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-sms-voice-v2", "put-registration-field-value")]
public record AwsPinpointSmsVoiceV2PutRegistrationFieldValueOptions(
[property: CliOption("--registration-id")] string RegistrationId,
[property: CliOption("--field-path")] string FieldPath
) : AwsOptions
{
    [CliOption("--select-choices")]
    public string[]? SelectChoices { get; set; }

    [CliOption("--text-value")]
    public string? TextValue { get; set; }

    [CliOption("--registration-attachment-id")]
    public string? RegistrationAttachmentId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}