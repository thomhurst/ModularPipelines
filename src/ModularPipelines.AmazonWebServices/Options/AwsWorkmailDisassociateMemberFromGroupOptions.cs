using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workmail", "disassociate-member-from-group")]
public record AwsWorkmailDisassociateMemberFromGroupOptions(
[property: CommandSwitch("--organization-id")] string OrganizationId,
[property: CommandSwitch("--group-id")] string GroupId,
[property: CommandSwitch("--member-id")] string MemberId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}