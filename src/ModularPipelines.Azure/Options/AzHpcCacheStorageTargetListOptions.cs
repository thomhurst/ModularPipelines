using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("hpc-cache", "storage-target", "list")]
public record AzHpcCacheStorageTargetListOptions(
[property: CommandSwitch("--cache-name")] string CacheName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
}