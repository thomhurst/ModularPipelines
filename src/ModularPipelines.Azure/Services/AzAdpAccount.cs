using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("adp")]
public class AzAdpAccount
{
    public AzAdpAccount(
        AzAdpAccountDataPool dataPool,
        ICommand internalCommand
    )
    {
        DataPool = dataPool;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzAdpAccountDataPool DataPool { get; }

    public async Task<CommandResult> Create(AzAdpAccountCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzAdpAccountDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAdpAccountDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzAdpAccountListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAdpAccountListOptions(), token);
    }

    public async Task<CommandResult> Show(AzAdpAccountShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAdpAccountShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzAdpAccountUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAdpAccountUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzAdpAccountWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAdpAccountWaitOptions(), token);
    }
}

