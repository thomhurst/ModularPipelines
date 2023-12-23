using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ses", "update-custom-verification-email-template")]
public record AwsSesUpdateCustomVerificationEmailTemplateOptions(
[property: CommandSwitch("--template-name")] string TemplateName
) : AwsOptions
{
    [CommandSwitch("--from-email-address")]
    public string? FromEmailAddress { get; set; }

    [CommandSwitch("--template-subject")]
    public string? TemplateSubject { get; set; }

    [CommandSwitch("--template-content")]
    public string? TemplateContent { get; set; }

    [CommandSwitch("--success-redirection-url")]
    public string? SuccessRedirectionUrl { get; set; }

    [CommandSwitch("--failure-redirection-url")]
    public string? FailureRedirectionUrl { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}