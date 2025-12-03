using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("synapse", "managed-private-endpoints", "create")]
public record AzSynapseManagedPrivateEndpointsCreateOptions(
[property: CliOption("--file")] string File,
[property: CliOption("--pe-name")] string PeName,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions;