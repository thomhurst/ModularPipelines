using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wellarchitected", "update-share-invitation")]
public record AwsWellarchitectedUpdateShareInvitationOptions(
[property: CommandSwitch("--share-invitation-id")] string ShareInvitationId,
[property: CommandSwitch("--share-invitation-action")] string ShareInvitationAction
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}