using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse", "link-connection", "list")]
public record AzSynapseLinkConnectionListOptions(
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions;