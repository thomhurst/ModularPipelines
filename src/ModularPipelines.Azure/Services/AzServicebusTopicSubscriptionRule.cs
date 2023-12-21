using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicebus", "topic", "subscription")]
public class AzServicebusTopicSubscriptionRule
{
    public AzServicebusTopicSubscriptionRule(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzServicebusTopicSubscriptionRuleCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzServicebusTopicSubscriptionRuleDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzServicebusTopicSubscriptionRuleDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzServicebusTopicSubscriptionRuleListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzServicebusTopicSubscriptionRuleShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzServicebusTopicSubscriptionRuleShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzServicebusTopicSubscriptionRuleUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzServicebusTopicSubscriptionRuleUpdateOptions(), token);
    }
}