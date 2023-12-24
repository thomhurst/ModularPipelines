using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "update-template-active-version")]
public record AwsPinpointUpdateTemplateActiveVersionOptions(
[property: CommandSwitch("--template-active-version-request")] string TemplateActiveVersionRequest,
[property: CommandSwitch("--template-name")] string TemplateName,
[property: CommandSwitch("--template-type")] string TemplateType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}