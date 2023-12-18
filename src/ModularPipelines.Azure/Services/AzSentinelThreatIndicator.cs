using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sentinel")]
public class AzSentinelThreatIndicator
{
    public AzSentinelThreatIndicator(
        AzSentinelThreatIndicatorMetric metric,
        ICommand internalCommand
    )
    {
        Metric = metric;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSentinelThreatIndicatorMetric Metric { get; }

    public async Task<CommandResult> AppendTag(AzSentinelThreatIndicatorAppendTagOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzSentinelThreatIndicatorCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzSentinelThreatIndicatorDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzSentinelThreatIndicatorListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Query(AzSentinelThreatIndicatorQueryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ReplaceTag(AzSentinelThreatIndicatorReplaceTagOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzSentinelThreatIndicatorShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSentinelThreatIndicatorShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzSentinelThreatIndicatorUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSentinelThreatIndicatorUpdateOptions(), token);
    }
}