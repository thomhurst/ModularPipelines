using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse", "sql-script", "list")]
public record AzSynapseSqlScriptListOptions(
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions;