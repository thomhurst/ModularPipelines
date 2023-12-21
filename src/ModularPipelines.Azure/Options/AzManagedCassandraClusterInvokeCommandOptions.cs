using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managed-cassandra", "cluster", "invoke-command")]
public record AzManagedCassandraClusterInvokeCommandOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--command-name")] string CommandName,
[property: CommandSwitch("--host")] string Host,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--arguments")]
    public string? ManagedCassandraClusterInvokeCommandArguments { get; set; }

    [BooleanCommandSwitch("--cassandra-stop-start")]
    public bool? CassandraStopStart { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [BooleanCommandSwitch("--readwrite")]
    public bool? Readwrite { get; set; }
}