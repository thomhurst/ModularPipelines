using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stack-hci-vm")]
public class AzStackHciVmStoragepath
{
    public AzStackHciVmStoragepath(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzStackHciVmStoragepathCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzStackHciVmStoragepathDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciVmStoragepathDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzStackHciVmStoragepathListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciVmStoragepathListOptions(), token);
    }

    public async Task<CommandResult> Show(AzStackHciVmStoragepathShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciVmStoragepathShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzStackHciVmStoragepathUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciVmStoragepathUpdateOptions(), token);
    }
}