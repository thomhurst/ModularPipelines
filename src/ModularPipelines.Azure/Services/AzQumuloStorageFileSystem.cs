using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("qumulo", "storage")]
public class AzQumuloStorageFileSystem
{
    public AzQumuloStorageFileSystem(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzQumuloStorageFileSystemCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzQumuloStorageFileSystemDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzQumuloStorageFileSystemDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzQumuloStorageFileSystemListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzQumuloStorageFileSystemListOptions(), token);
    }

    public async Task<CommandResult> Show(AzQumuloStorageFileSystemShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzQumuloStorageFileSystemShowOptions(), token);
    }

    public async Task<CommandResult> Wait(AzQumuloStorageFileSystemWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzQumuloStorageFileSystemWaitOptions(), token);
    }
}