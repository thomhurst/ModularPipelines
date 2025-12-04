using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse", "kusto", "pool", "update")]
public record AzSynapseKustoPoolUpdateOptions : AzOptions
{
    [CliFlag("--enable-purge")]
    public bool? EnablePurge { get; set; }

    [CliFlag("--enable-streaming-ingest")]
    public bool? EnableStreamingIngest { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliOption("--kusto-pool-name")]
    public string? KustoPoolName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--optimized-autoscale")]
    public string? OptimizedAutoscale { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }

    [CliOption("--workspace-uid")]
    public string? WorkspaceUid { get; set; }
}