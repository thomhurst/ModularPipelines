using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse", "data-flow", "list")]
public record AzSynapseDataFlowListOptions(
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions;