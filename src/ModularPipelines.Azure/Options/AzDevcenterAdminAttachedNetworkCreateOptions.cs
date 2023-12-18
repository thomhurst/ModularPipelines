using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devcenter", "admin", "attached-network", "create")]
public record AzDevcenterAdminAttachedNetworkCreateOptions(
[property: BooleanCommandSwitch("--attached-network-connection-name")] bool AttachedNetworkConnectionName,
[property: CommandSwitch("--dev-center")] string DevCenter,
[property: CommandSwitch("--network-connection-id")] string NetworkConnectionId,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}