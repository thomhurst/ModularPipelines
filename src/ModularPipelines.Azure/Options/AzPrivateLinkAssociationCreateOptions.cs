using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("private-link", "association", "create")]
public record AzPrivateLinkAssociationCreateOptions(
[property: CommandSwitch("--management-group-id")] string ManagementGroupId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--privatelink")] string Privatelink,
[property: CommandSwitch("--public-network-access")] string PublicNetworkAccess
) : AzOptions;