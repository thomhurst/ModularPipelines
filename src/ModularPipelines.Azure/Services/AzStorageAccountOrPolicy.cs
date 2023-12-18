using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "account")]
public class AzStorageAccountOrPolicy
{
    public AzStorageAccountOrPolicy(
        AzStorageAccountOrPolicyRule rule,
        ICommand internalCommand
    )
    {
        Rule = rule;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzStorageAccountOrPolicyRule Rule { get; }

    public async Task<CommandResult> Create(AzStorageAccountOrPolicyCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzStorageAccountOrPolicyDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzStorageAccountOrPolicyListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzStorageAccountOrPolicyShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzStorageAccountOrPolicyUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}