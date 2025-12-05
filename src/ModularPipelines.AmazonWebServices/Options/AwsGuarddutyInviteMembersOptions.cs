using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("guardduty", "invite-members")]
public record AwsGuarddutyInviteMembersOptions(
[property: CliOption("--detector-id")] string DetectorId,
[property: CliOption("--account-ids")] string[] AccountIds
) : AwsOptions
{
    [CliOption("--message")]
    public string? Message { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}