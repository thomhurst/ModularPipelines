using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workmail", "disassociate-member-from-group")]
public record AwsWorkmailDisassociateMemberFromGroupOptions(
[property: CliOption("--organization-id")] string OrganizationId,
[property: CliOption("--group-id")] string GroupId,
[property: CliOption("--member-id")] string MemberId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}