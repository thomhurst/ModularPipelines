using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicebus")]
public class AzServicebusQueue
{
    public AzServicebusQueue(
        AzServicebusQueueAuthorizationRule authorizationRule,
        ICommand internalCommand
    )
    {
        AuthorizationRule = authorizationRule;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzServicebusQueueAuthorizationRule AuthorizationRule { get; }

    public async Task<CommandResult> Create(AzServicebusQueueCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzServicebusQueueDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzServicebusQueueListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzServicebusQueueShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzServicebusQueueShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzServicebusQueueUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzServicebusQueueUpdateOptions(), token);
    }
}