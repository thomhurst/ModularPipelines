using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("hdinsight")]
public class AzHdinsightAutoscale
{
    public AzHdinsightAutoscale(
        AzHdinsightAutoscaleCondition condition,
        ICommand internalCommand
    )
    {
        Condition = condition;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzHdinsightAutoscaleCondition Condition { get; }

    public async Task<CommandResult> Create(AzHdinsightAutoscaleCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzHdinsightAutoscaleDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTimezones(AzHdinsightAutoscaleListTimezonesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHdinsightAutoscaleListTimezonesOptions(), token);
    }

    public async Task<CommandResult> Show(AzHdinsightAutoscaleShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzHdinsightAutoscaleUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzHdinsightAutoscaleWaitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}