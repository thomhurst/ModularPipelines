using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("monitor")]
public class AzMonitorMetrics
{
    public AzMonitorMetrics(
        AzMonitorMetricsAlert alert,
        ICommand internalCommand
    )
    {
        Alert = alert;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzMonitorMetricsAlert Alert { get; }

    public async Task<CommandResult> List(AzMonitorMetricsListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListDefinitions(AzMonitorMetricsListDefinitionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListNamespaces(AzMonitorMetricsListNamespacesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}