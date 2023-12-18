using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("databoxedge", "bandwidth-schedule", "list")]
public record AzDataboxedgeBandwidthScheduleListOptions(
[property: CommandSwitch("--device-name")] string DeviceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;