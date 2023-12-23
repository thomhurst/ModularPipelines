using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("macie2", "accept-invitation")]
public record AwsMacie2AcceptInvitationOptions(
[property: CommandSwitch("--invitation-id")] string InvitationId
) : AwsOptions
{
    [CommandSwitch("--administrator-account-id")]
    public string? AdministratorAccountId { get; set; }

    [CommandSwitch("--master-account")]
    public string? MasterAccount { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}