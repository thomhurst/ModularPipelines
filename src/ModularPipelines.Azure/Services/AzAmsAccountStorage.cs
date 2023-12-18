using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ams", "account")]
public class AzAmsAccountStorage
{
    public AzAmsAccountStorage(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Add(AzAmsAccountStorageAddOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsAccountStorageAddOptions(), token);
    }

    public async Task<CommandResult> Remove(AzAmsAccountStorageRemoveOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsAccountStorageRemoveOptions(), token);
    }

    public async Task<CommandResult> SetAuthentication(AzAmsAccountStorageSetAuthenticationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SyncStorageKeys(AzAmsAccountStorageSyncStorageKeysOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}