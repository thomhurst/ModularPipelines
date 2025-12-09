using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ses", "send-custom-verification-email")]
public record AwsSesSendCustomVerificationEmailOptions(
[property: CliOption("--email-address")] string EmailAddress,
[property: CliOption("--template-name")] string TemplateName
) : AwsOptions
{
    [CliOption("--configuration-set-name")]
    public string? ConfigurationSetName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}