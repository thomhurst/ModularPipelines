using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "diagnostic-settings")]
public class AzMonitorDiagnosticSettingsSubscription
{
    public AzMonitorDiagnosticSettingsSubscription(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzMonitorDiagnosticSettingsSubscriptionCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzMonitorDiagnosticSettingsSubscriptionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorDiagnosticSettingsSubscriptionDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzMonitorDiagnosticSettingsSubscriptionListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorDiagnosticSettingsSubscriptionListOptions(), token);
    }

    public async Task<CommandResult> Show(AzMonitorDiagnosticSettingsSubscriptionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorDiagnosticSettingsSubscriptionShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzMonitorDiagnosticSettingsSubscriptionUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorDiagnosticSettingsSubscriptionUpdateOptions(), token);
    }
}