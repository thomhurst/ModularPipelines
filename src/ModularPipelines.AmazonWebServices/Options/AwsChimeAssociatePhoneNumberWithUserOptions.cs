using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime", "associate-phone-number-with-user")]
public record AwsChimeAssociatePhoneNumberWithUserOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--user-id")] string UserId,
[property: CliOption("--e164-phone-number")] string E164PhoneNumber
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}