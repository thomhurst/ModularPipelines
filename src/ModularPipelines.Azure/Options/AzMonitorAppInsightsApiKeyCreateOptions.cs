using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "app-insights", "api-key", "create")]
public record AzMonitorAppInsightsApiKeyCreateOptions(
[property: CommandSwitch("--api-key")] string ApiKey,
[property: CommandSwitch("--app")] string App,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--read-properties")]
    public string? ReadProperties { get; set; }

    [CommandSwitch("--write-properties")]
    public string? WriteProperties { get; set; }
}

