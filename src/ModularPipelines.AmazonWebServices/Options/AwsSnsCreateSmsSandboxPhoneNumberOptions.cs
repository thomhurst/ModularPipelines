using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sns", "create-sms-sandbox-phone-number")]
public record AwsSnsCreateSmsSandboxPhoneNumberOptions(
[property: CommandSwitch("--phone-number")] string PhoneNumber
) : AwsOptions
{
    [CommandSwitch("--language-code")]
    public string? LanguageCode { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}