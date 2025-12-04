using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("connectedmachine", "private-endpoint-connection", "list")]
public record AzConnectedmachinePrivateEndpointConnectionListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--scope-name")] string ScopeName
) : AzOptions;