using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime", "associate-signin-delegate-groups-with-account")]
public record AwsChimeAssociateSigninDelegateGroupsWithAccountOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--signin-delegate-groups")] string[] SigninDelegateGroups
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}