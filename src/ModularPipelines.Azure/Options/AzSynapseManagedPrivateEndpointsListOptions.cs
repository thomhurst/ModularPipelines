using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("synapse", "managed-private-endpoints", "list")]
public record AzSynapseManagedPrivateEndpointsListOptions(
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions;