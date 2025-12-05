using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("identity", "groups", "memberships", "get-membership-graph")]
public record GcloudIdentityGroupsMembershipsGetMembershipGraphOptions(
[property: CliOption("--labels")] string Labels,
[property: CliOption("--member-email")] string MemberEmail
) : GcloudOptions
{
    [CliOption("--group-email")]
    public string? GroupEmail { get; set; }
}