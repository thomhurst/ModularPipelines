using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-sms-voice-v2", "create-registration-attachment")]
public record AwsPinpointSmsVoiceV2CreateRegistrationAttachmentOptions : AwsOptions
{
    [CliOption("--attachment-body")]
    public string? AttachmentBody { get; set; }

    [CliOption("--attachment-url")]
    public string? AttachmentUrl { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}