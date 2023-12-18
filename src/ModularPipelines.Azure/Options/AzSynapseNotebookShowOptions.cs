using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "notebook", "show")]
public record AzSynapseNotebookShowOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions;