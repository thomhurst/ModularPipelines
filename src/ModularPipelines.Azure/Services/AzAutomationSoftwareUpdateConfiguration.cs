using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("automation")]
public class AzAutomationSoftwareUpdateConfiguration
{
    public AzAutomationSoftwareUpdateConfiguration(
        AzAutomationSoftwareUpdateConfigurationMachineRuns machineRuns,
        AzAutomationSoftwareUpdateConfigurationRuns runs,
        ICommand internalCommand
    )
    {
        MachineRuns = machineRuns;
        Runs = runs;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzAutomationSoftwareUpdateConfigurationMachineRuns MachineRuns { get; }

    public AzAutomationSoftwareUpdateConfigurationRuns Runs { get; }

    public async Task<CommandResult> Create(AzAutomationSoftwareUpdateConfigurationCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzAutomationSoftwareUpdateConfigurationDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzAutomationSoftwareUpdateConfigurationListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzAutomationSoftwareUpdateConfigurationShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}