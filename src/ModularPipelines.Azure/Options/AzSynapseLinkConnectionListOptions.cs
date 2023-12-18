using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "link-connection", "list")]
public record AzSynapseLinkConnectionListOptions(
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
}