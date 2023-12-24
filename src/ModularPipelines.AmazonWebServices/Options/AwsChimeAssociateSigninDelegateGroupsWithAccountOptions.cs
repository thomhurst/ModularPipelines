using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime", "associate-signin-delegate-groups-with-account")]
public record AwsChimeAssociateSigninDelegateGroupsWithAccountOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--signin-delegate-groups")] string[] SigninDelegateGroups
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}