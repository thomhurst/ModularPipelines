using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stack-hci-vm")]
public class AzStackHciVmDisk
{
    public AzStackHciVmDisk(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Attach(AzStackHciVmDiskAttachOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciVmDiskAttachOptions(), token);
    }

    public async Task<CommandResult> Create(AzStackHciVmDiskCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzStackHciVmDiskDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciVmDiskDeleteOptions(), token);
    }

    public async Task<CommandResult> Detach(AzStackHciVmDiskDetachOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzStackHciVmDiskListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciVmDiskListOptions(), token);
    }

    public async Task<CommandResult> Show(AzStackHciVmDiskShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciVmDiskShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzStackHciVmDiskUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciVmDiskUpdateOptions(), token);
    }
}