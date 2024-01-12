using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("identity", "groups", "memberships", "list")]
public record GcloudIdentityGroupsMembershipsListOptions(
[property: CommandSwitch("--group-email")] string GroupEmail
) : GcloudOptions
{
    [CommandSwitch("--page-token")]
    public string? PageToken { get; set; }

    [CommandSwitch("--view")]
    public string? View { get; set; }
}