using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("identity", "groups", "memberships", "list")]
public record GcloudIdentityGroupsMembershipsListOptions(
[property: CliOption("--group-email")] string GroupEmail
) : GcloudOptions
{
    [CliOption("--page-token")]
    public string? PageToken { get; set; }

    [CliOption("--view")]
    public string? View { get; set; }
}