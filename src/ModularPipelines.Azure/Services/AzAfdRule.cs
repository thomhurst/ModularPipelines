using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("afd")]
public class AzAfdRule
{
    public AzAfdRule(
        AzAfdRuleAction action,
        AzAfdRuleCondition condition,
        ICommand internalCommand
    )
    {
        Action = action;
        Condition = condition;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzAfdRuleAction Action { get; }

    public AzAfdRuleCondition Condition { get; }

    public async Task<CommandResult> Create(AzAfdRuleCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzAfdRuleDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzAfdRuleListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzAfdRuleShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAfdRuleShowOptions(), token);
    }
}