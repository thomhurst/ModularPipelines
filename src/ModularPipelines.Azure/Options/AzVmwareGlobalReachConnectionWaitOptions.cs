using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vmware", "global-reach-connection", "wait")]
public record AzVmwareGlobalReachConnectionWaitOptions : AzOptions
{
    [CliFlag("--created")]
    public bool? Created { get; set; }

    [CliOption("--custom")]
    public string? Custom { get; set; }

    [CliFlag("--deleted")]
    public bool? Deleted { get; set; }

    [CliFlag("--exists")]
    public bool? Exists { get; set; }

    [CliOption("--global-reach-connection-name")]
    public string? GlobalReachConnectionName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--interval")]
    public int? Interval { get; set; }

    [CliOption("--private-cloud")]
    public string? PrivateCloud { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliFlag("--updated")]
    public bool? Updated { get; set; }
}