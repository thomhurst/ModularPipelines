using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sesv2", "update-email-template")]
public record AwsSesv2UpdateEmailTemplateOptions(
[property: CommandSwitch("--template-name")] string TemplateName,
[property: CommandSwitch("--template-content")] string TemplateContent
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}