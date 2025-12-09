using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "mi", "link", "update")]
public record AzSqlMiLinkUpdateOptions : AzOptions
{
    [CliOption("--distributed-availability-group-name")]
    public string? DistributedAvailabilityGroupName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--instance-name")]
    public string? InstanceName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--replication-mode")]
    public string? ReplicationMode { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}