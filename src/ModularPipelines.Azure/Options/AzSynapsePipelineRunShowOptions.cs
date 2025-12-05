using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse", "pipeline-run", "show")]
public record AzSynapsePipelineRunShowOptions(
[property: CliOption("--run-id")] string RunId,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions;