using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectedmachine", "private-link-resource", "list")]
public record AzConnectedmachinePrivateLinkResourceListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--scope-name")] string ScopeName
) : AzOptions;