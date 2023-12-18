using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicebus")]
public class AzServicebusTopic
{
    public AzServicebusTopic(
        AzServicebusTopicAuthorizationRule authorizationRule,
        AzServicebusTopicSubscription subscription,
        ICommand internalCommand
    )
    {
        AuthorizationRule = authorizationRule;
        Subscription = subscription;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzServicebusTopicAuthorizationRule AuthorizationRule { get; }

    public AzServicebusTopicSubscription Subscription { get; }

    public async Task<CommandResult> Create(AzServicebusTopicCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzServicebusTopicDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzServicebusTopicListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzServicebusTopicShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzServicebusTopicShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzServicebusTopicUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzServicebusTopicUpdateOptions(), token);
    }
}