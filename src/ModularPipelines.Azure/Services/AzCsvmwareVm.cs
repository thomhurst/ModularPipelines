using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("csvmware")]
public class AzCsvmwareVm
{
    public AzCsvmwareVm(
        AzCsvmwareVmDisk disk,
        AzCsvmwareVmNic nic,
        ICommand internalCommand
    )
    {
        Disk = disk;
        Nic = nic;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzCsvmwareVmDisk Disk { get; }

    public AzCsvmwareVmNic Nic { get; }

    public async Task<CommandResult> Create(AzCsvmwareVmCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzCsvmwareVmDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzCsvmwareVmListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCsvmwareVmListOptions(), token);
    }

    public async Task<CommandResult> Show(AzCsvmwareVmShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Start(AzCsvmwareVmStartOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Stop(AzCsvmwareVmStopOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzCsvmwareVmUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}