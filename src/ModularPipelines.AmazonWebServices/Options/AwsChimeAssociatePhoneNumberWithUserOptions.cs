using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime", "associate-phone-number-with-user")]
public record AwsChimeAssociatePhoneNumberWithUserOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--user-id")] string UserId,
[property: CommandSwitch("--e164-phone-number")] string E164PhoneNumber
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}