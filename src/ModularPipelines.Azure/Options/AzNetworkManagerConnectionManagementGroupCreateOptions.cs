using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "manager", "connection", "management-group", "create")]
public record AzNetworkManagerConnectionManagementGroupCreateOptions(
[property: CommandSwitch("--connection-name")] string ConnectionName,
[property: CommandSwitch("--management-group-id")] string ManagementGroupId,
[property: CommandSwitch("--network-manager")] string NetworkManager
) : AzOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }
}