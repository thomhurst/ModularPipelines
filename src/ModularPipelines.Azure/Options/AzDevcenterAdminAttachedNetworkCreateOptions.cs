using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("devcenter", "admin", "attached-network", "create")]
public record AzDevcenterAdminAttachedNetworkCreateOptions(
[property: CliFlag("--attached-network-connection-name")] bool AttachedNetworkConnectionName,
[property: CliOption("--dev-center")] string DevCenter,
[property: CliOption("--network-connection-id")] string NetworkConnectionId,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}