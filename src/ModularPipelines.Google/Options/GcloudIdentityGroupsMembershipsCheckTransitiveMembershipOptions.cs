using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("identity", "groups", "memberships", "check-transitive-membership")]
public record GcloudIdentityGroupsMembershipsCheckTransitiveMembershipOptions(
[property: CliOption("--group-email")] string GroupEmail,
[property: CliOption("--member-email")] string MemberEmail
) : GcloudOptions;