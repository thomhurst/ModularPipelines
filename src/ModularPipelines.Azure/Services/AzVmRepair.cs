using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("vm")]
public class AzVmRepair
{
    public AzVmRepair(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzVmRepairCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListScripts(AzVmRepairListScriptsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmRepairListScriptsOptions(), token);
    }

    public async Task<CommandResult> RepairAndRestore(AzVmRepairRepairAndRestoreOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmRepairRepairAndRestoreOptions(), token);
    }

    public async Task<CommandResult> ResetNic(AzVmRepairResetNicOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmRepairResetNicOptions(), token);
    }

    public async Task<CommandResult> Restore(AzVmRepairRestoreOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmRepairRestoreOptions(), token);
    }

    public async Task<CommandResult> Run(AzVmRepairRunOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmRepairRunOptions(), token);
    }
}