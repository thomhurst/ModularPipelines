using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("identity", "groups", "memberships", "search-transitive-memberships")]
public record GcloudIdentityGroupsMembershipsSearchTransitiveMembershipsOptions(
[property: CommandSwitch("--group-email")] string GroupEmail
) : GcloudOptions
{
    [CommandSwitch("--page-size")]
    public string? PageSize { get; set; }

    [CommandSwitch("--page-token")]
    public string? PageToken { get; set; }
}