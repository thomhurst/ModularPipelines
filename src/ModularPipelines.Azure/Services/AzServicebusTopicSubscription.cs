using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicebus", "topic")]
public class AzServicebusTopicSubscription
{
    public AzServicebusTopicSubscription(
        AzServicebusTopicSubscriptionRule rule,
        ICommand internalCommand
    )
    {
        Rule = rule;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzServicebusTopicSubscriptionRule Rule { get; }

    public async Task<CommandResult> Create(AzServicebusTopicSubscriptionCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzServicebusTopicSubscriptionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzServicebusTopicSubscriptionDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzServicebusTopicSubscriptionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzServicebusTopicSubscriptionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzServicebusTopicSubscriptionShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzServicebusTopicSubscriptionUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzServicebusTopicSubscriptionUpdateOptions(), token);
    }
}