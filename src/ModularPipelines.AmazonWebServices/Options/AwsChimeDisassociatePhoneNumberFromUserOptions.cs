using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime", "disassociate-phone-number-from-user")]
public record AwsChimeDisassociatePhoneNumberFromUserOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--user-id")] string UserId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}