using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sns", "check-if-phone-number-is-opted-out")]
public record AwsSnsCheckIfPhoneNumberIsOptedOutOptions(
[property: CliOption("--phone-number")] string PhoneNumber
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}