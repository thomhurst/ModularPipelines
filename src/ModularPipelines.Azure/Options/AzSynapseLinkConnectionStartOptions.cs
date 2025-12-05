using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse", "link-connection", "start")]
public record AzSynapseLinkConnectionStartOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions;