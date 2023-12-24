using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("guardduty", "accept-administrator-invitation")]
public record AwsGuarddutyAcceptAdministratorInvitationOptions(
[property: CommandSwitch("--detector-id")] string DetectorId,
[property: CommandSwitch("--administrator-id")] string AdministratorId,
[property: CommandSwitch("--invitation-id")] string InvitationId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}