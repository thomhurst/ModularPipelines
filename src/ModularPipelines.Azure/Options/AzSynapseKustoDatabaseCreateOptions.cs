using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "kusto", "database", "create")]
public record AzSynapseKustoDatabaseCreateOptions(
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--kusto-pool-name")] string KustoPoolName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--read-write-database")]
    public string? ReadWriteDatabase { get; set; }
}

