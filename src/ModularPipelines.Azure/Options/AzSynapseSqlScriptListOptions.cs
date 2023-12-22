using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "sql-script", "list")]
public record AzSynapseSqlScriptListOptions(
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions;