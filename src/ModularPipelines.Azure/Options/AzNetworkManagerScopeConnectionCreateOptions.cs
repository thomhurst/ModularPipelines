using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "manager", "scope-connection", "create")]
public record AzNetworkManagerScopeConnectionCreateOptions(
[property: CliOption("--connection-name")] string ConnectionName,
[property: CliOption("--network-manager")] string NetworkManager,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--tenant-id")] string TenantId
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }
}