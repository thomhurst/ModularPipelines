using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sesv2", "test-render-email-template")]
public record AwsSesv2TestRenderEmailTemplateOptions(
[property: CommandSwitch("--template-name")] string TemplateName,
[property: CommandSwitch("--template-data")] string TemplateData
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}