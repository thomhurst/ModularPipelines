using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "dataset", "list")]
public record AzSynapseDatasetListOptions(
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
}