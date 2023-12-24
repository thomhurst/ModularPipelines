using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "update-in-app-template")]
public record AwsPinpointUpdateInAppTemplateOptions(
[property: CommandSwitch("--in-app-template-request")] string InAppTemplateRequest,
[property: CommandSwitch("--template-name")] string TemplateName
) : AwsOptions
{
    [CommandSwitch("--template-version")]
    public string? TemplateVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}