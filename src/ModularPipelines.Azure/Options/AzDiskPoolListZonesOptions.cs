using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("disk-pool", "list-zones")]
public record AzDiskPoolListZonesOptions(
[property: CliOption("--location")] string Location
) : AzOptions;