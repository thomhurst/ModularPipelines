using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("servicebus", "topic")]
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
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzServicebusTopicAuthorizationRuleDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzServicebusTopicAuthorizationRuleDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzServicebusTopicAuthorizationRuleListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzServicebusTopicAuthorizationRuleShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzServicebusTopicAuthorizationRuleShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzServicebusTopicAuthorizationRuleUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzServicebusTopicAuthorizationRuleUpdateOptions(), cancellationToken: token);
    }
}