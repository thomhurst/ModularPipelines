using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sesv2", "update-custom-verification-email-template")]
public record AwsSesv2UpdateCustomVerificationEmailTemplateOptions(
[property: CliOption("--template-name")] string TemplateName,
[property: CliOption("--from-email-address")] string FromEmailAddress,
[property: CliOption("--template-subject")] string TemplateSubject,
[property: CliOption("--template-content")] string TemplateContent,
[property: CliOption("--success-redirection-url")] string SuccessRedirectionUrl,
[property: CliOption("--failure-redirection-url")] string FailureRedirectionUrl
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}