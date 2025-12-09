using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("guardduty", "accept-administrator-invitation")]
public record AwsGuarddutyAcceptAdministratorInvitationOptions(
[property: CliOption("--detector-id")] string DetectorId,
[property: CliOption("--administrator-id")] string AdministratorId,
[property: CliOption("--invitation-id")] string InvitationId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}