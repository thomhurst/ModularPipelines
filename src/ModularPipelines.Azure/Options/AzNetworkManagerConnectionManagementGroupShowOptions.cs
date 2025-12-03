using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "manager", "connection", "management-group", "show")]
public record AzNetworkManagerConnectionManagementGroupShowOptions(
[property: CliOption("--connection-name")] string ConnectionName,
[property: CliOption("--management-group-id")] string ManagementGroupId
) : AzOptions;