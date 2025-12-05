using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("identity", "groups", "memberships", "search-transitive-memberships")]
public record GcloudIdentityGroupsMembershipsSearchTransitiveMembershipsOptions(
[property: CliOption("--group-email")] string GroupEmail
) : GcloudOptions
{
    [CliOption("--page-size")]
    public string? PageSize { get; set; }

    [CliOption("--page-token")]
    public string? PageToken { get; set; }
}