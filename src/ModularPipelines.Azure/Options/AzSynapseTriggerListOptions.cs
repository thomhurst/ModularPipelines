using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse", "trigger", "list")]
public record AzSynapseTriggerListOptions(
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions;