using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzHealthbot
{
    public AzHealthbot(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzHealthbotCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzHealthbotDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHealthbotDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzHealthbotListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHealthbotListOptions(), token);
    }

    public async Task<CommandResult> Show(AzHealthbotShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHealthbotShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzHealthbotUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHealthbotUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzHealthbotWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHealthbotWaitOptions(), token);
    }
}