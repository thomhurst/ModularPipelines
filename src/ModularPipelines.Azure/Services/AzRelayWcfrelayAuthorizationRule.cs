using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("relay", "wcfrelay")]
public class AzRelayWcfrelayAuthorizationRule
{
    public AzRelayWcfrelayAuthorizationRule(
        AzRelayWcfrelayAuthorizationRuleKeys keys,
        ICommand internalCommand
    )
    {
        Keys = keys;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzRelayWcfrelayAuthorizationRuleKeys Keys { get; }

    public async Task<CommandResult> Create(AzRelayWcfrelayAuthorizationRuleCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzRelayWcfrelayAuthorizationRuleDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzRelayWcfrelayAuthorizationRuleDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzRelayWcfrelayAuthorizationRuleListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzRelayWcfrelayAuthorizationRuleShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzRelayWcfrelayAuthorizationRuleShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzRelayWcfrelayAuthorizationRuleUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}