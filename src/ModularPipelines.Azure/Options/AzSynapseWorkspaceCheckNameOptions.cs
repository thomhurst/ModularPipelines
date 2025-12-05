using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse", "workspace", "check-name")]
public record AzSynapseWorkspaceCheckNameOptions(
[property: CliOption("--name")] string Name
) : AzOptions;