using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("securityhub", "accept-administrator-invitation")]
public record AwsSecurityhubAcceptAdministratorInvitationOptions(
[property: CommandSwitch("--administrator-id")] string AdministratorId,
[property: CommandSwitch("--invitation-id")] string InvitationId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}