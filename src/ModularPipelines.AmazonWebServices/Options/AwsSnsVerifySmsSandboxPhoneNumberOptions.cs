using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sns", "verify-sms-sandbox-phone-number")]
public record AwsSnsVerifySmsSandboxPhoneNumberOptions(
[property: CommandSwitch("--phone-number")] string PhoneNumber,
[property: CommandSwitch("--one-time-password")] string OneTimePassword
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}