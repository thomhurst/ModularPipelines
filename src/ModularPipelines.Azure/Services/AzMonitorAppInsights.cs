using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor")]
public class AzMonitorAppInsights
{
    public AzMonitorAppInsights(
        AzMonitorAppInsightsApiKey apiKey,
        AzMonitorAppInsightsComponent component,
        AzMonitorAppInsightsEvents events,
        AzMonitorAppInsightsMetrics metrics,
        AzMonitorAppInsightsWebTest webTest,
        ICommand internalCommand
    )
    {
        ApiKey = apiKey;
        Component = component;
        Events = events;
        Metrics = metrics;
        WebTest = webTest;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzMonitorAppInsightsApiKey ApiKey { get; }

    public AzMonitorAppInsightsComponent Component { get; }

    public AzMonitorAppInsightsEvents Events { get; }

    public AzMonitorAppInsightsMetrics Metrics { get; }

    public AzMonitorAppInsightsWebTest WebTest { get; }

    public async Task<CommandResult> Query(AzMonitorAppInsightsQueryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}