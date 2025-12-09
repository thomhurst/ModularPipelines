using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("connectedmachine", "private-link-resource", "list")]
public record AzConnectedmachinePrivateLinkResourceListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--scope-name")] string ScopeName
) : AzOptions;