using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sesv2", "put-account-details")]
public record AwsSesv2PutAccountDetailsOptions(
[property: CommandSwitch("--mail-type")] string MailType,
[property: CommandSwitch("--website-url")] string WebsiteUrl,
[property: CommandSwitch("--use-case-description")] string UseCaseDescription
) : AwsOptions
{
    [CommandSwitch("--contact-language")]
    public string? ContactLanguage { get; set; }

    [CommandSwitch("--additional-contact-email-addresses")]
    public string[]? AdditionalContactEmailAddresses { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}