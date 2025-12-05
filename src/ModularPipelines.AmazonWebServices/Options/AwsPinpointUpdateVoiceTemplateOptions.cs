using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "update-voice-template")]
public record AwsPinpointUpdateVoiceTemplateOptions(
[property: CliOption("--template-name")] string TemplateName,
[property: CliOption("--voice-template-request")] string VoiceTemplateRequest
) : AwsOptions
{
    [CliOption("--template-version")]
    public string? TemplateVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}