using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "update-email-template")]
public record AwsPinpointUpdateEmailTemplateOptions(
[property: CliOption("--email-template-request")] string EmailTemplateRequest,
[property: CliOption("--template-name")] string TemplateName
) : AwsOptions
{
    [CliOption("--template-version")]
    public string? TemplateVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}