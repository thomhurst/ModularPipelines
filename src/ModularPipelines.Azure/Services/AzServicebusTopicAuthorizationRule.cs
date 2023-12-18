using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicebus", "topic")]
public class AzServicebusTopicAuthorizationRule
{
    public AzServicebusTopicAuthorizationRule(
        AzServicebusTopicAuthorizationRuleKeys keys,
        ICommand internalCommand
    )
    {
        Keys = keys;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzServicebusTopicAuthorizationRuleKeys Keys { get; }

    public async Task<CommandResult> Create(AzServicebusTopicAuthorizationRuleCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzServicebusTopicAuthorizationRuleDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzServicebusTopicAuthorizationRuleListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzServicebusTopicAuthorizationRuleShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzServicebusTopicAuthorizationRuleShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzServicebusTopicAuthorizationRuleUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzServicebusTopicAuthorizationRuleUpdateOptions(), token);
    }
}