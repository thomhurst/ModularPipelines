using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventhubs", "namespace")]
public class AzEventhubsNamespaceAuthorizationRule
{
    public AzEventhubsNamespaceAuthorizationRule(
        AzEventhubsNamespaceAuthorizationRuleKeys keys,
        ICommand internalCommand
    )
    {
        Keys = keys;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzEventhubsNamespaceAuthorizationRuleKeys Keys { get; }

    public async Task<CommandResult> Create(AzEventhubsNamespaceAuthorizationRuleCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzEventhubsNamespaceAuthorizationRuleDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventhubsNamespaceAuthorizationRuleDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzEventhubsNamespaceAuthorizationRuleListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzEventhubsNamespaceAuthorizationRuleShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventhubsNamespaceAuthorizationRuleShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzEventhubsNamespaceAuthorizationRuleUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventhubsNamespaceAuthorizationRuleUpdateOptions(), token);
    }
}