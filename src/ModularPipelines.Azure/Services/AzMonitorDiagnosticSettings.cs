using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor")]
public class AzMonitorDiagnosticSettings
{
    public AzMonitorDiagnosticSettings(
        AzMonitorDiagnosticSettingsCategories categories,
        AzMonitorDiagnosticSettingsSubscription subscription,
        ICommand internalCommand
    )
    {
        Categories = categories;
        Subscription = subscription;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzMonitorDiagnosticSettingsCategories Categories { get; }

    public AzMonitorDiagnosticSettingsSubscription Subscription { get; }

    public async Task<CommandResult> Create(AzMonitorDiagnosticSettingsCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzMonitorDiagnosticSettingsDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzMonitorDiagnosticSettingsListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzMonitorDiagnosticSettingsShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzMonitorDiagnosticSettingsUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}