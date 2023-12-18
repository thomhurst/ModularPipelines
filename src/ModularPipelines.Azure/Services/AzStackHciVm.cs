using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzStackHciVmDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciVmDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzStackHciVmListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciVmListOptions(), token);
    }

    public async Task<CommandResult> Restart(AzStackHciVmRestartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciVmRestartOptions(), token);
    }

    public async Task<CommandResult> Show(AzStackHciVmShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciVmShowOptions(), token);
    }

    public async Task<CommandResult> Start(AzStackHciVmStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciVmStartOptions(), token);
    }

    public async Task<CommandResult> Stop(AzStackHciVmStopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciVmStopOptions(), token);
    }

    public async Task<CommandResult> Update(AzStackHciVmUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciVmUpdateOptions(), token);
    }
}