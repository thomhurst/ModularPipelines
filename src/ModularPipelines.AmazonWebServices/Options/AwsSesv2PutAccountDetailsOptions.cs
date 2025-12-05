using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sesv2", "put-account-details")]
public record AwsSesv2PutAccountDetailsOptions(
[property: CliOption("--mail-type")] string MailType,
[property: CliOption("--website-url")] string WebsiteUrl,
[property: CliOption("--use-case-description")] string UseCaseDescription
) : AwsOptions
{
    [CliOption("--contact-language")]
    public string? ContactLanguage { get; set; }

    [CliOption("--additional-contact-email-addresses")]
    public string[]? AdditionalContactEmailAddresses { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}