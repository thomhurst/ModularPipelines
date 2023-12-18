using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "sql-script", "delete")]
public record AzSynapseSqlScriptDeleteOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}