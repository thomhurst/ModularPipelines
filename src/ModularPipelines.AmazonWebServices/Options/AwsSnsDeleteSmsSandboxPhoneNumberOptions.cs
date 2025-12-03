using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sns", "delete-sms-sandbox-phone-number")]
public record AwsSnsDeleteSmsSandboxPhoneNumberOptions(
[property: CliOption("--phone-number")] string PhoneNumber
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}