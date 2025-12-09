using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse", "notebook", "list")]
public record AzSynapseNotebookListOptions(
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions;