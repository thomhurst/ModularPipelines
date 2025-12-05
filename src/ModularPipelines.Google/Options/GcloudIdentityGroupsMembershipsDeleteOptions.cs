using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("identity", "groups", "memberships", "delete")]
public record GcloudIdentityGroupsMembershipsDeleteOptions(
[property: CliOption("--group-email")] string GroupEmail,
[property: CliOption("--member-email")] string MemberEmail
) : GcloudOptions;