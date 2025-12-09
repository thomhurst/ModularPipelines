using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vmware", "hcx-enterprise-site", "show")]
public record AzVmwareHcxEnterpriseSiteShowOptions : AzOptions
{
    [CliOption("--hcx-enterprise-site-name")]
    public string? HcxEnterpriseSiteName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--private-cloud")]
    public string? PrivateCloud { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}