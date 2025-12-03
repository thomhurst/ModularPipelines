using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("synapse", "sql", "pool", "restore")]
public record AzSynapseSqlPoolRestoreOptions(
[property: CliOption("--dest-name")] string DestName
) : AzOptions
{
    [CliOption("--deleted-time")]
    public string? DeletedTime { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--performance-level")]
    public string? PerformanceLevel { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--storage-type")]
    public string? StorageType { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--time")]
    public string? Time { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}