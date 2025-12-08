using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("monitor")]
public class AzMonitorAlertProcessingRule
{
    public AzMonitorAlertProcessingRule(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzMonitorAlertProcessingRuleCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzMonitorAlertProcessingRuleDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorAlertProcessingRuleDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzMonitorAlertProcessingRuleListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorAlertProcessingRuleListOptions(), token);
    }

    public async Task<CommandResult> Show(AzMonitorAlertProcessingRuleShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorAlertProcessingRuleShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzMonitorAlertProcessingRuleUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorAlertProcessingRuleUpdateOptions(), token);
    }
}