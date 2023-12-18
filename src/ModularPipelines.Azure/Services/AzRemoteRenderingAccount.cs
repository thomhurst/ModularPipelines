using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzRemoteRenderingAccount
{
    public AzRemoteRenderingAccount(
        AzRemoteRenderingAccountKey key,
        ICommand internalCommand
    )
    {
        Key = key;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzRemoteRenderingAccountKey Key { get; }

    public async Task<CommandResult> Create(AzRemoteRenderingAccountCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzRemoteRenderingAccountDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzRemoteRenderingAccountDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzRemoteRenderingAccountListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzRemoteRenderingAccountListOptions(), token);
    }

    public async Task<CommandResult> Show(AzRemoteRenderingAccountShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzRemoteRenderingAccountShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzRemoteRenderingAccountUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzRemoteRenderingAccountUpdateOptions(), token);
    }
}

