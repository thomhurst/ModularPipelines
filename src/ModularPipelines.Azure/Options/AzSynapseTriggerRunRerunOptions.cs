using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "trigger-run", "rerun")]
public record AzSynapseTriggerRunRerunOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--run-id")] string RunId,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
}

