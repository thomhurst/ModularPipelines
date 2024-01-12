using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("identity", "groups", "memberships", "delete")]
public record GcloudIdentityGroupsMembershipsDeleteOptions(
[property: CommandSwitch("--group-email")] string GroupEmail,
[property: CommandSwitch("--member-email")] string MemberEmail
) : GcloudOptions;