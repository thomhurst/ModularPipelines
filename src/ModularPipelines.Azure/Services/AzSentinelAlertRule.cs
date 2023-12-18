using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sentinel")]
public class AzSentinelAlertRule
{
    public AzSentinelAlertRule(
        AzSentinelAlertRuleAction action,
        AzSentinelAlertRuleTemplate template,
        ICommand internalCommand
    )
    {
        Action = action;
        Template = template;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSentinelAlertRuleAction Action { get; }

    public AzSentinelAlertRuleTemplate Template { get; }

    public async Task<CommandResult> Create(AzSentinelAlertRuleCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzSentinelAlertRuleDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzSentinelAlertRuleListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzSentinelAlertRuleShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSentinelAlertRuleShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzSentinelAlertRuleUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSentinelAlertRuleUpdateOptions(), token);
    }
}