using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ses", "update-custom-verification-email-template")]
public record AwsSesUpdateCustomVerificationEmailTemplateOptions(
[property: CliOption("--template-name")] string TemplateName
) : AwsOptions
{
    [CliOption("--from-email-address")]
    public string? FromEmailAddress { get; set; }

    [CliOption("--template-subject")]
    public string? TemplateSubject { get; set; }

    [CliOption("--template-content")]
    public string? TemplateContent { get; set; }

    [CliOption("--success-redirection-url")]
    public string? SuccessRedirectionUrl { get; set; }

    [CliOption("--failure-redirection-url")]
    public string? FailureRedirectionUrl { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}