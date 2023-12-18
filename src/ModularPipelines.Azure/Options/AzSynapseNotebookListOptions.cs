using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "notebook", "list")]
public record AzSynapseNotebookListOptions(
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions;