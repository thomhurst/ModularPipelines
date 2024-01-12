using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "instances", "promote-replica")]
public record GcloudSqlInstancesPromoteReplicaOptions(
[property: PositionalArgument] string Replica
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--[no-]failover")]
    public string[]? NoFailover { get; set; }
}