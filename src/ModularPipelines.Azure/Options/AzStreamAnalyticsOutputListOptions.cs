using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stream-analytics", "output", "list")]
public record AzStreamAnalyticsOutputListOptions(
[property: CommandSwitch("--job-name")] string JobName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--select")]
    public string? Select { get; set; }
}