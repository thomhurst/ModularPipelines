using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dynatrace", "monitor", "list")]
public record AzDynatraceMonitorListOptions(
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;