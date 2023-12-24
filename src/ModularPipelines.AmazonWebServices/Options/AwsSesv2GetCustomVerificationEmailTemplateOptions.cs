using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sesv2", "get-custom-verification-email-template")]
public record AwsSesv2GetCustomVerificationEmailTemplateOptions(
[property: CommandSwitch("--template-name")] string TemplateName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}