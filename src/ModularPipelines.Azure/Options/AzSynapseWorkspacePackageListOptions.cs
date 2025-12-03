using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("synapse", "workspace-package", "list")]
public record AzSynapseWorkspacePackageListOptions(
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions;