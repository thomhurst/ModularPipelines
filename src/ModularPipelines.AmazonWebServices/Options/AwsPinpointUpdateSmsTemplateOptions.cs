using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "update-sms-template")]
public record AwsPinpointUpdateSmsTemplateOptions(
[property: CommandSwitch("--sms-template-request")] string SmsTemplateRequest,
[property: CommandSwitch("--template-name")] string TemplateName
) : AwsOptions
{
    [CommandSwitch("--template-version")]
    public string? TemplateVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}