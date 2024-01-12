using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("identity", "groups", "memberships", "add")]
public record GcloudIdentityGroupsMembershipsAddOptions(
[property: CommandSwitch("--group-email")] string GroupEmail,
[property: CommandSwitch("--member-email")] string MemberEmail
) : GcloudOptions
{
    [CommandSwitch("--expiration")]
    public string? Expiration { get; set; }

    [CommandSwitch("--roles")]
    public string[]? Roles { get; set; }
}