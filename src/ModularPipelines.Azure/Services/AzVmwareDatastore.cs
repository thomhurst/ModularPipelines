using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware")]
public class AzVmwareDatastore
{
    public AzVmwareDatastore(
        AzVmwareDatastoreDiskPoolVolume diskPoolVolume,
        AzVmwareDatastoreNetappVolume netappVolume,
        ICommand internalCommand
    )
    {
        DiskPoolVolume = diskPoolVolume;
        NetappVolume = netappVolume;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzVmwareDatastoreDiskPoolVolume DiskPoolVolume { get; }

    public AzVmwareDatastoreNetappVolume NetappVolume { get; }

    public async Task<CommandResult> Delete(AzVmwareDatastoreDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmwareDatastoreDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzVmwareDatastoreListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzVmwareDatastoreShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmwareDatastoreShowOptions(), token);
    }

    public async Task<CommandResult> Wait(AzVmwareDatastoreWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmwareDatastoreWaitOptions(), token);
    }
}