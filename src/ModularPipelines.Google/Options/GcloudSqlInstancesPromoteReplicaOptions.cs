using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "instances", "promote-replica")]
public record GcloudSqlInstancesPromoteReplicaOptions(
[property: CliArgument] string Replica
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--[no-]failover")]
    public string[]? NoFailover { get; set; }
}