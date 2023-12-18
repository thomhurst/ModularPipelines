using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "app-insights")]
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
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ConnectWebapp(AzMonitorAppInsightsComponentConnectWebappOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzMonitorAppInsightsComponentCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzMonitorAppInsightsComponentDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzMonitorAppInsightsComponentShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzMonitorAppInsightsComponentUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateTags(AzMonitorAppInsightsComponentUpdateTagsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}