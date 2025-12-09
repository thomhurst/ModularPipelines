using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sns", "verify-sms-sandbox-phone-number")]
public record AwsSnsVerifySmsSandboxPhoneNumberOptions(
[property: CliOption("--phone-number")] string PhoneNumber,
[property: CliOption("--one-time-password")] string OneTimePassword
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}