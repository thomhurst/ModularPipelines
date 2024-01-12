using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("identity", "groups", "memberships", "get-membership-graph")]
public record GcloudIdentityGroupsMembershipsGetMembershipGraphOptions(
[property: CommandSwitch("--labels")] string Labels,
[property: CommandSwitch("--member-email")] string MemberEmail
) : GcloudOptions
{
    [CommandSwitch("--group-email")]
    public string? GroupEmail { get; set; }
}