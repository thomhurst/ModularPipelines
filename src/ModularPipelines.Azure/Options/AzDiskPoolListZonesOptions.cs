using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("disk-pool", "list-zones")]
public record AzDiskPoolListZonesOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions;