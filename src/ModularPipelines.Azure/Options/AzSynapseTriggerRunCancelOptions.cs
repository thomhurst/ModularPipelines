using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse", "trigger-run", "cancel")]
public record AzSynapseTriggerRunCancelOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--run-id")] string RunId,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions;