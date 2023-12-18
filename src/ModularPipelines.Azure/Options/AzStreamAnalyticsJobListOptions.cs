using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stream-analytics", "job", "list")]
public record AzStreamAnalyticsJobListOptions : AzOptions
{
    [CommandSwitch("--expand")]
    public string? Expand { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}