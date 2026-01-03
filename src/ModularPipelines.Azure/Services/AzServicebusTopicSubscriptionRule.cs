using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("servicebus", "topic", "subscription")]
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
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzServicebusTopicSubscriptionRuleDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzServicebusTopicSubscriptionRuleDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzServicebusTopicSubscriptionRuleListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzServicebusTopicSubscriptionRuleShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzServicebusTopicSubscriptionRuleShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzServicebusTopicSubscriptionRuleUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzServicebusTopicSubscriptionRuleUpdateOptions(), cancellationToken: token);
    }
}