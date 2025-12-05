using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("identity", "groups", "memberships", "add")]
public record GcloudIdentityGroupsMembershipsAddOptions(
[property: CliOption("--group-email")] string GroupEmail,
[property: CliOption("--member-email")] string MemberEmail
) : GcloudOptions
{
    [CliOption("--expiration")]
    public string? Expiration { get; set; }

    [CliOption("--roles")]
    public string[]? Roles { get; set; }
}