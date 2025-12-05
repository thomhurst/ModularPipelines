using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("repostspace", "send-invites")]
public record AwsRepostspaceSendInvitesOptions(
[property: CliOption("--accessor-ids")] string[] AccessorIds,
[property: CliOption("--body")] string Body,
[property: CliOption("--space-id")] string SpaceId,
[property: CliOption("--title")] string Title
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}