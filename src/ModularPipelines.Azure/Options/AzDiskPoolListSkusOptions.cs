using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("disk-pool", "list-skus")]
public record AzDiskPoolListSkusOptions(
[property: CliOption("--location")] string Location
) : AzOptions;