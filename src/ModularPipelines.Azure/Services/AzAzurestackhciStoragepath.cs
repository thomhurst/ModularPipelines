using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("urestackhci")]
public class AzAzurestackhciStoragepath
{
    public AzAzurestackhciStoragepath(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzAzurestackhciStoragepathCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzAzurestackhciStoragepathDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAzurestackhciStoragepathDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzAzurestackhciStoragepathListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAzurestackhciStoragepathListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzAzurestackhciStoragepathShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAzurestackhciStoragepathShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzAzurestackhciStoragepathUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAzurestackhciStoragepathUpdateOptions(), cancellationToken: token);
    }
}