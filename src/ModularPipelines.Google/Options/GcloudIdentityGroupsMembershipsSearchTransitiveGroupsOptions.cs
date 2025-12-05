using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("identity", "groups", "memberships", "search-transitive-groups")]
public record GcloudIdentityGroupsMembershipsSearchTransitiveGroupsOptions(
[property: CliOption("--labels")] string Labels,
[property: CliOption("--member-email")] string MemberEmail
) : GcloudOptions
{
    [CliOption("--page-size")]
    public string? PageSize { get; set; }

    [CliOption("--page-token")]
    public string? PageToken { get; set; }
}