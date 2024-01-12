using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("identity", "groups", "memberships", "search-transitive-groups")]
public record GcloudIdentityGroupsMembershipsSearchTransitiveGroupsOptions(
[property: CommandSwitch("--labels")] string Labels,
[property: CommandSwitch("--member-email")] string MemberEmail
) : GcloudOptions
{
    [CommandSwitch("--page-size")]
    public string? PageSize { get; set; }

    [CommandSwitch("--page-token")]
    public string? PageToken { get; set; }
}