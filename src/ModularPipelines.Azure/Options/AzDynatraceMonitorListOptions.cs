using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dynatrace", "monitor", "list")]
public record AzDynatraceMonitorListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
}