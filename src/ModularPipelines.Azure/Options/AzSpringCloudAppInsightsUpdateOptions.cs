using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring-cloud", "app-insights", "update")]
public record AzSpringCloudAppInsightsUpdateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--app-insights")]
    public string? AppInsights { get; set; }

    [CommandSwitch("--app-insights-key")]
    public string? AppInsightsKey { get; set; }

    [BooleanCommandSwitch("--disable")]
    public bool? Disable { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--sampling-rate")]
    public string? SamplingRate { get; set; }
}

