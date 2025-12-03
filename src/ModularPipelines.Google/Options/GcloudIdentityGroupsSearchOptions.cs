using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("identity", "groups", "search")]
public record GcloudIdentityGroupsSearchOptions(
[property: CliOption("--labels")] string Labels,
[property: CliOption("--customer")] string Customer,
[property: CliOption("--organization")] string Organization
) : GcloudOptions
{
    [CliOption("--page-size")]
    public string? PageSize { get; set; }

    [CliOption("--page-token")]
    public string? PageToken { get; set; }

    [CliOption("--view")]
    public string? View { get; set; }
}