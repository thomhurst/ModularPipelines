using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "link-connection", "list")]
public record AzSynapseLinkConnectionListOptions(
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions;