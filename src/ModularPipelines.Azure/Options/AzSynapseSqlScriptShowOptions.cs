using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "sql-script", "show")]
public record AzSynapseSqlScriptShowOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions;