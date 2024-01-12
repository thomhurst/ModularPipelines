using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("identity", "groups", "memberships", "check-transitive-membership")]
public record GcloudIdentityGroupsMembershipsCheckTransitiveMembershipOptions(
[property: CommandSwitch("--group-email")] string GroupEmail,
[property: CommandSwitch("--member-email")] string MemberEmail
) : GcloudOptions;