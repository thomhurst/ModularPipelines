using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sns", "create-sms-sandbox-phone-number")]
public record AwsSnsCreateSmsSandboxPhoneNumberOptions(
[property: CliOption("--phone-number")] string PhoneNumber
) : AwsOptions
{
    [CliOption("--language-code")]
    public string? LanguageCode { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}