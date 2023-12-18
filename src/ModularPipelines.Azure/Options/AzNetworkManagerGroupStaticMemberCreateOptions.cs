using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "manager", "group", "static-member", "create")]
public record AzNetworkManagerGroupStaticMemberCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--network-group")] string NetworkGroup,
[property: CommandSwitch("--network-manager")] string NetworkManager,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--resource-id")] string ResourceId
) : AzOptions;