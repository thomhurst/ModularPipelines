using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "create-voice-template")]
public record AwsPinpointCreateVoiceTemplateOptions(
[property: CliOption("--template-name")] string TemplateName,
[property: CliOption("--voice-template-request")] string VoiceTemplateRequest
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}