using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicebus", "queue")]
public class AzServicebusQueueAuthorizationRule
{
    public AzServicebusQueueAuthorizationRule(
        AzServicebusQueueAuthorizationRuleKeys keys,
        ICommand internalCommand
    )
    {
        Keys = keys;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzServicebusQueueAuthorizationRuleKeys Keys { get; }

    public async Task<CommandResult> Create(AzServicebusQueueAuthorizationRuleCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzServicebusQueueAuthorizationRuleDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzServicebusQueueAuthorizationRuleDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzServicebusQueueAuthorizationRuleListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzServicebusQueueAuthorizationRuleShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzServicebusQueueAuthorizationRuleShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzServicebusQueueAuthorizationRuleUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzServicebusQueueAuthorizationRuleUpdateOptions(), token);
    }
}