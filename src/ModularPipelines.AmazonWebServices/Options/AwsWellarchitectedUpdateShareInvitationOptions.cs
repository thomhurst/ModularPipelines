using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wellarchitected", "update-share-invitation")]
public record AwsWellarchitectedUpdateShareInvitationOptions(
[property: CliOption("--share-invitation-id")] string ShareInvitationId,
[property: CliOption("--share-invitation-action")] string ShareInvitationAction
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}