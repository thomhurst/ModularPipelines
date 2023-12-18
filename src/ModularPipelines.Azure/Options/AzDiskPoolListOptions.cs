using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("disk-pool", "list")]
public record AzDiskPoolListOptions(
[property: CommandSwitch("--disk-pool-name")] string DiskPoolName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
}