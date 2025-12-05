using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("policy")]
public class AzPolicyState
{
    public AzPolicyState(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> List(AzPolicyStateListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPolicyStateListOptions(), token);
    }

    public async Task<CommandResult> Summarize(AzPolicyStateSummarizeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPolicyStateSummarizeOptions(), token);
    }

    public async Task<CommandResult> TriggerScan(AzPolicyStateTriggerScanOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPolicyStateTriggerScanOptions(), token);
    }
}