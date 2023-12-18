using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("adp", "account")]
public class AzAdpAccountDataPool
{
    public AzAdpAccountDataPool(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzAdpAccountDataPoolCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzAdpAccountDataPoolDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAdpAccountDataPoolDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzAdpAccountDataPoolListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzAdpAccountDataPoolShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAdpAccountDataPoolShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzAdpAccountDataPoolUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAdpAccountDataPoolUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzAdpAccountDataPoolWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAdpAccountDataPoolWaitOptions(), token);
    }
}