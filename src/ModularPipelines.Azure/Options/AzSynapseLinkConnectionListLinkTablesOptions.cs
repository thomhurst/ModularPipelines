using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse", "link-connection", "list-link-tables")]
public record AzSynapseLinkConnectionListLinkTablesOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions;