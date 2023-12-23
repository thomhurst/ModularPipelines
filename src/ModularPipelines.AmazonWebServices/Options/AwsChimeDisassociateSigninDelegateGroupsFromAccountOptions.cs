using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime", "disassociate-signin-delegate-groups-from-account")]
public record AwsChimeDisassociateSigninDelegateGroupsFromAccountOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--group-names")] string[] GroupNames
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}