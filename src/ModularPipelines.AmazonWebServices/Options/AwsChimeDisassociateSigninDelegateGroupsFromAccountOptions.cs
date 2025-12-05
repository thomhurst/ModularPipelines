using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime", "disassociate-signin-delegate-groups-from-account")]
public record AwsChimeDisassociateSigninDelegateGroupsFromAccountOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--group-names")] string[] GroupNames
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}