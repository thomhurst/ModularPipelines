using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "manager", "connection", "management-group", "delete")]
public record AzNetworkManagerConnectionManagementGroupDeleteOptions(
[property: CommandSwitch("--connection-name")] string ConnectionName,
[property: CommandSwitch("--management-group-id")] string ManagementGroupId
) : AzOptions
{
    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}

