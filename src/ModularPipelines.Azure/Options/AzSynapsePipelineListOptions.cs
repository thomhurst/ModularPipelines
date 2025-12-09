using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse", "pipeline", "list")]
public record AzSynapsePipelineListOptions(
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions;