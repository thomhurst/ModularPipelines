using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("signalr", "replica", "delete")]
public record AzSignalrReplicaDeleteOptions : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--replica-name")]
    public string? ReplicaName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--signalr-name")]
    public string? SignalrName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}