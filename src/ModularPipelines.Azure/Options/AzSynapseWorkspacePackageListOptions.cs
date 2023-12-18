using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "workspace-package", "list")]
public record AzSynapseWorkspacePackageListOptions(
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
}