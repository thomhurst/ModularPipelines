using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workmail", "associate-member-to-group")]
public record AwsWorkmailAssociateMemberToGroupOptions(
[property: CliOption("--organization-id")] string OrganizationId,
[property: CliOption("--group-id")] string GroupId,
[property: CliOption("--member-id")] string MemberId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}