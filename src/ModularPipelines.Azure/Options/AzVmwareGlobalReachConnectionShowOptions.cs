using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "global-reach-connection", "show")]
public record AzVmwareGlobalReachConnectionShowOptions : AzOptions
{
    [CliOption("--global-reach-connection-name")]
    public string? GlobalReachConnectionName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--private-cloud")]
    public string? PrivateCloud { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}