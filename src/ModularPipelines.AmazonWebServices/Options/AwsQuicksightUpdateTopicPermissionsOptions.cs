using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "update-topic-permissions")]
public record AwsQuicksightUpdateTopicPermissionsOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--topic-id")] string TopicId
) : AwsOptions
{
    [CliOption("--grant-permissions")]
    public string[]? GrantPermissions { get; set; }

    [CliOption("--revoke-permissions")]
    public string[]? RevokePermissions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}