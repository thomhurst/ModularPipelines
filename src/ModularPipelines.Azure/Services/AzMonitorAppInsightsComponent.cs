using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "app-insights")]
public class AzMonitorAppInsightsComponent
{
    public AzMonitorAppInsightsComponent(
        AzMonitorAppInsightsComponentBilling billing,
        AzMonitorAppInsightsComponentContinuesExport continuesExport,
        AzMonitorAppInsightsComponentLinkedStorage linkedStorage,
        ICommand internalCommand
    )
    {
        Billing = billing;
        ContinuesExport = continuesExport;
        LinkedStorage = linkedStorage;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzMonitorAppInsightsComponentBilling Billing { get; }

    public AzMonitorAppInsightsComponentContinuesExport ContinuesExport { get; }

    public AzMonitorAppInsightsComponentLinkedStorage LinkedStorage { get; }

    public async Task<CommandResult> ConnectFunction(AzMonitorAppInsightsComponentConnectFunctionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> ConnectWebapp(AzMonitorAppInsightsComponentConnectWebappOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Create(AzMonitorAppInsightsComponentCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzMonitorAppInsightsComponentDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorAppInsightsComponentDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzMonitorAppInsightsComponentShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorAppInsightsComponentShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzMonitorAppInsightsComponentUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorAppInsightsComponentUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> UpdateTags(AzMonitorAppInsightsComponentUpdateTagsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }
}