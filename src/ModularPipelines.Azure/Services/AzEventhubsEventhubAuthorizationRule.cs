using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventhubs", "eventhub")]
public class AzEventhubsEventhubAuthorizationRule
{
    public AzEventhubsEventhubAuthorizationRule(
        AzEventhubsEventhubAuthorizationRuleKeys keys,
        ICommand internalCommand
    )
    {
        Keys = keys;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzEventhubsEventhubAuthorizationRuleKeys Keys { get; }

    public async Task<CommandResult> Create(AzEventhubsEventhubAuthorizationRuleCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzEventhubsEventhubAuthorizationRuleDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventhubsEventhubAuthorizationRuleDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzEventhubsEventhubAuthorizationRuleListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzEventhubsEventhubAuthorizationRuleShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventhubsEventhubAuthorizationRuleShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzEventhubsEventhubAuthorizationRuleUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventhubsEventhubAuthorizationRuleUpdateOptions(), token);
    }
}