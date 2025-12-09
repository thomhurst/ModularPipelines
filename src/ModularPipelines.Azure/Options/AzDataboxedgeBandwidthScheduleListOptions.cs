using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("databoxedge", "bandwidth-schedule", "list")]
public record AzDataboxedgeBandwidthScheduleListOptions(
[property: CliOption("--device-name")] string DeviceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;