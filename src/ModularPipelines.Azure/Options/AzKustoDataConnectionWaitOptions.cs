using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kusto", "data-connection", "wait")]
public record AzKustoDataConnectionWaitOptions : AzOptions
{
    [CliOption("--cluster-name")]
    public string? ClusterName { get; set; }

    [CliFlag("--created")]
    public bool? Created { get; set; }

    [CliOption("--custom")]
    public string? Custom { get; set; }

    [CliOption("--data-connection-name")]
    public string? DataConnectionName { get; set; }

    [CliOption("--database-name")]
    public string? DatabaseName { get; set; }

    [CliFlag("--deleted")]
    public bool? Deleted { get; set; }

    [CliFlag("--exists")]
    public bool? Exists { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--interval")]
    public int? Interval { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliFlag("--updated")]
    public bool? Updated { get; set; }
}