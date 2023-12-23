using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime", "invite-users")]
public record AwsChimeInviteUsersOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--user-email-list")] string[] UserEmailList
) : AwsOptions
{
    [CommandSwitch("--user-type")]
    public string? UserType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}