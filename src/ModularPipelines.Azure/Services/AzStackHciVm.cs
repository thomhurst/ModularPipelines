using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzStackHciVm
{
    public AzStackHciVm(
        AzStackHciVmDisk disk,
        AzStackHciVmImage image,
        AzStackHciVmNetwork network,
        AzStackHciVmNic nic,
        AzStackHciVmStoragepath storagepath,
        ICommand internalCommand
    )
    {
        Disk = disk;
        Image = image;
        Network = network;
        Nic = nic;
        Storagepath = storagepath;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzStackHciVmDisk Disk { get; }

    public AzStackHciVmImage Image { get; }

    public AzStackHciVmNetwork Network { get; }

    public AzStackHciVmNic Nic { get; }

    public AzStackHciVmStoragepath Storagepath { get; }

    public async Task<CommandResult> Create(AzStackHciVmCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzStackHciVmDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciVmDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzStackHciVmListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciVmListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Restart(AzStackHciVmRestartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciVmRestartOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzStackHciVmShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciVmShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Start(AzStackHciVmStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciVmStartOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Stop(AzStackHciVmStopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciVmStopOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzStackHciVmUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciVmUpdateOptions(), cancellationToken: token);
    }
}