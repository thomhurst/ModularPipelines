using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "manager", "scope-connection", "create")]
public record AzNetworkManagerScopeConnectionCreateOptions(
[property: CommandSwitch("--connection-name")] string ConnectionName,
[property: CommandSwitch("--network-manager")] string NetworkManager,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--tenant-id")] string TenantId
) : AzOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }
}

