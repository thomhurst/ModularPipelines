using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("dynatrace", "monitor")]
public class AzDynatraceMonitorTagRule
{
    public AzDynatraceMonitorTagRule(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzDynatraceMonitorTagRuleCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzDynatraceMonitorTagRuleDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDynatraceMonitorTagRuleDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzDynatraceMonitorTagRuleListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzDynatraceMonitorTagRuleShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDynatraceMonitorTagRuleShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzDynatraceMonitorTagRuleUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDynatraceMonitorTagRuleUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzDynatraceMonitorTagRuleWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDynatraceMonitorTagRuleWaitOptions(), cancellationToken: token);
    }
}