using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("identity", "groups", "memberships", "modify-membership-roles")]
public record GcloudIdentityGroupsMembershipsModifyMembershipRolesOptions(
[property: CommandSwitch("--group-email")] string GroupEmail,
[property: CommandSwitch("--member-email")] string MemberEmail
) : GcloudOptions
{
    [CommandSwitch("--update-roles-params")]
    public string? UpdateRolesParams { get; set; }

    [CommandSwitch("--add-roles")]
    public string? AddRoles { get; set; }

    [CommandSwitch("--remove-roles")]
    public string[]? RemoveRoles { get; set; }
}