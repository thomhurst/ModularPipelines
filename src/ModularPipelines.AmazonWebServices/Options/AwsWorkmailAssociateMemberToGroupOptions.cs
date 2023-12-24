using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workmail", "associate-member-to-group")]
public record AwsWorkmailAssociateMemberToGroupOptions(
[property: CommandSwitch("--organization-id")] string OrganizationId,
[property: CommandSwitch("--group-id")] string GroupId,
[property: CommandSwitch("--member-id")] string MemberId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}