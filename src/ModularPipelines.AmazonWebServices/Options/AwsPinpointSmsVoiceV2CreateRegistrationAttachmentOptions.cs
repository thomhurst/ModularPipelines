using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint-sms-voice-v2", "create-registration-attachment")]
public record AwsPinpointSmsVoiceV2CreateRegistrationAttachmentOptions : AwsOptions
{
    [CommandSwitch("--attachment-body")]
    public string? AttachmentBody { get; set; }

    [CommandSwitch("--attachment-url")]
    public string? AttachmentUrl { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}