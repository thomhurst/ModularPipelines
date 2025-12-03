using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("managed-cassandra", "cluster", "invoke-command")]
public record AzManagedCassandraClusterInvokeCommandOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--command-name")] string CommandName,
[property: CliOption("--host")] string Host,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--arguments")]
    public string? ManagedCassandraClusterInvokeCommandArguments { get; set; }

    [CliFlag("--cassandra-stop-start")]
    public bool? CassandraStopStart { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliFlag("--readwrite")]
    public bool? Readwrite { get; set; }
}