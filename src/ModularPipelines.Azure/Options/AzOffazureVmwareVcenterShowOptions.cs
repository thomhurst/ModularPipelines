using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("offure", "vmware", "vcenter", "show")]
public record AzOffazureVmwareVcenterShowOptions : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--site-name")]
    public string? SiteName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}