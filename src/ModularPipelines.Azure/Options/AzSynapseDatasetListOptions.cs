using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse", "dataset", "list")]
public record AzSynapseDatasetListOptions(
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions;