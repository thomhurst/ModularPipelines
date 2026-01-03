using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("stack-hci-vm")]
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
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzStackHciVmStoragepathDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciVmStoragepathDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzStackHciVmStoragepathListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciVmStoragepathListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzStackHciVmStoragepathShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciVmStoragepathShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzStackHciVmStoragepathUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciVmStoragepathUpdateOptions(), cancellationToken: token);
    }
}