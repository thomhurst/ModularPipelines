using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("stack-hci-vm")]
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
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciVmDiskAttachOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Create(AzStackHciVmDiskCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzStackHciVmDiskDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciVmDiskDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Detach(AzStackHciVmDiskDetachOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> List(AzStackHciVmDiskListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciVmDiskListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzStackHciVmDiskShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciVmDiskShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzStackHciVmDiskUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciVmDiskUpdateOptions(), cancellationToken: token);
    }
}