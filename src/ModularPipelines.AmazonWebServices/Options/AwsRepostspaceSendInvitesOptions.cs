using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("repostspace", "send-invites")]
public record AwsRepostspaceSendInvitesOptions(
[property: CommandSwitch("--accessor-ids")] string[] AccessorIds,
[property: CommandSwitch("--body")] string Body,
[property: CommandSwitch("--space-id")] string SpaceId,
[property: CommandSwitch("--title")] string Title
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}