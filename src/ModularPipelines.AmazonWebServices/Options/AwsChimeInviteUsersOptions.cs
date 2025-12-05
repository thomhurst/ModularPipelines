using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime", "invite-users")]
public record AwsChimeInviteUsersOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--user-email-list")] string[] UserEmailList
) : AwsOptions
{
    [CliOption("--user-type")]
    public string? UserType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}