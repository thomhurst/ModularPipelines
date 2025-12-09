using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vm", "list-usage")]
public record AzVmListUsageOptions(
[property: CliOption("--location")] string Location
) : AzOptions;