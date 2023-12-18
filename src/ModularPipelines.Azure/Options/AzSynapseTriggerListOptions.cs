using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "trigger", "list")]
public record AzSynapseTriggerListOptions(
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions;