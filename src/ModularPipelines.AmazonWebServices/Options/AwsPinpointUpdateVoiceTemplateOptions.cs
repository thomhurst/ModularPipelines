using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "update-voice-template")]
public record AwsPinpointUpdateVoiceTemplateOptions(
[property: CommandSwitch("--template-name")] string TemplateName,
[property: CommandSwitch("--voice-template-request")] string VoiceTemplateRequest
) : AwsOptions
{
    [CommandSwitch("--template-version")]
    public string? TemplateVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}