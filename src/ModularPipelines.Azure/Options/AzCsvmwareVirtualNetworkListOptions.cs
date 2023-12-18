using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("csvmware", "virtual-network", "list")]
public record AzCsvmwareVirtualNetworkListOptions(
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--private-cloud")] string PrivateCloud,
[property: CommandSwitch("--resource-pool")] string ResourcePool
) : AzOptions
{
}

