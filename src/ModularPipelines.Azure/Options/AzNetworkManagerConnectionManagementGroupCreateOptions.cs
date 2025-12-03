using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "manager", "connection", "management-group", "create")]
public record AzNetworkManagerConnectionManagementGroupCreateOptions(
[property: CliOption("--connection-name")] string ConnectionName,
[property: CliOption("--management-group-id")] string ManagementGroupId,
[property: CliOption("--network-manager")] string NetworkManager
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }
}