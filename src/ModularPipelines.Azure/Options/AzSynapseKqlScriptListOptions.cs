using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "kql-script", "list")]
public record AzSynapseKqlScriptListOptions(
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions;