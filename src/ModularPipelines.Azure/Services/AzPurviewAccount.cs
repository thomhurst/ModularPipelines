using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("purview")]
public class AzPurviewAccount
{
    public AzPurviewAccount(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AddRootCollectionAdmin(AzPurviewAccountAddRootCollectionAdminOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPurviewAccountAddRootCollectionAdminOptions(), token);
    }

    public async Task<CommandResult> Create(AzPurviewAccountCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzPurviewAccountDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPurviewAccountDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzPurviewAccountListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPurviewAccountListOptions(), token);
    }

    public async Task<CommandResult> ListKey(AzPurviewAccountListKeyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzPurviewAccountShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPurviewAccountShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzPurviewAccountUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPurviewAccountUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzPurviewAccountWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPurviewAccountWaitOptions(), token);
    }
}