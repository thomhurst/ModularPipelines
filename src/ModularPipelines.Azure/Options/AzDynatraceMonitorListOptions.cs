using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dynatrace", "monitor", "list")]
public record AzDynatraceMonitorListOptions(
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;