using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("site-recovery", "vmware-site", "run-as-account", "list")]
public record AzSiteRecoveryVmwareSiteRunAsAccountListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--site-name")] string SiteName
) : AzOptions
{
    [CliOption("--max-items")]
    public string? MaxItems { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }
}