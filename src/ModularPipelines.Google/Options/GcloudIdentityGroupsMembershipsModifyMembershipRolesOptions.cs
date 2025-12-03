using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("identity", "groups", "memberships", "modify-membership-roles")]
public record GcloudIdentityGroupsMembershipsModifyMembershipRolesOptions(
[property: CliOption("--group-email")] string GroupEmail,
[property: CliOption("--member-email")] string MemberEmail
) : GcloudOptions
{
    [CliOption("--update-roles-params")]
    public string? UpdateRolesParams { get; set; }

    [CliOption("--add-roles")]
    public string? AddRoles { get; set; }

    [CliOption("--remove-roles")]
    public string[]? RemoveRoles { get; set; }
}