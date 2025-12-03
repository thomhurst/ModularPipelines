using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "manager", "connection", "management-group", "delete")]
public record AzNetworkManagerConnectionManagementGroupDeleteOptions(
[property: CliOption("--connection-name")] string ConnectionName,
[property: CliOption("--management-group-id")] string ManagementGroupId
) : AzOptions
{
    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}