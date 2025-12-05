using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "delete-folder-membership")]
public record AwsQuicksightDeleteFolderMembershipOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--folder-id")] string FolderId,
[property: CliOption("--member-id")] string MemberId,
[property: CliOption("--member-type")] string MemberType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}