using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("relay", "namespace")]
public class AzRelayNamespaceAuthorizationRule
{
    public AzRelayNamespaceAuthorizationRule(
        AzRelayNamespaceAuthorizationRuleKeys keys,
        ICommand internalCommand
    )
    {
        Keys = keys;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzRelayNamespaceAuthorizationRuleKeys Keys { get; }

    public async Task<CommandResult> Create(AzRelayNamespaceAuthorizationRuleCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzRelayNamespaceAuthorizationRuleDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzRelayNamespaceAuthorizationRuleDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzRelayNamespaceAuthorizationRuleListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzRelayNamespaceAuthorizationRuleShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzRelayNamespaceAuthorizationRuleShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzRelayNamespaceAuthorizationRuleUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzRelayNamespaceAuthorizationRuleUpdateOptions(), token);
    }
}