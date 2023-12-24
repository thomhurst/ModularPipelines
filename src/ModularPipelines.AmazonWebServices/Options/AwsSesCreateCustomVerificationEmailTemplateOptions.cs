using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ses", "create-custom-verification-email-template")]
public record AwsSesCreateCustomVerificationEmailTemplateOptions(
[property: CommandSwitch("--template-name")] string TemplateName,
[property: CommandSwitch("--from-email-address")] string FromEmailAddress,
[property: CommandSwitch("--template-subject")] string TemplateSubject,
[property: CommandSwitch("--template-content")] string TemplateContent,
[property: CommandSwitch("--success-redirection-url")] string SuccessRedirectionUrl,
[property: CommandSwitch("--failure-redirection-url")] string FailureRedirectionUrl
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}