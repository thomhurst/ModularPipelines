using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint-sms-voice-v2", "create-registration")]
public record AwsPinpointSmsVoiceV2CreateRegistrationOptions(
[property: CommandSwitch("--registration-type")] string RegistrationType
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}