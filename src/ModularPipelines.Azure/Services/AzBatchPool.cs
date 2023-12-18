using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch")]
public class AzBatchPool
{
    public AzBatchPool(
        AzBatchPoolAutoscale autoscale,
        AzBatchPoolCreate create,
        AzBatchPoolNodeCounts nodeCounts,
        AzBatchPoolSupportedImages supportedImages,
        AzBatchPoolUsageMetrics usageMetrics,
        ICommand internalCommand
    )
    {
        Autoscale = autoscale;
        CreateCommands = create;
        NodeCounts = nodeCounts;
        SupportedImages = supportedImages;
        UsageMetrics = usageMetrics;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzBatchPoolAutoscale Autoscale { get; }

    public AzBatchPoolCreate CreateCommands { get; }

    public AzBatchPoolNodeCounts NodeCounts { get; }

    public AzBatchPoolSupportedImages SupportedImages { get; }

    public AzBatchPoolUsageMetrics UsageMetrics { get; }

    public async Task<CommandResult> Create(AzBatchPoolCreateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzBatchPoolCreateOptions(), token);
    }

    public async Task<CommandResult> Delete(AzBatchPoolDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzBatchPoolListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzBatchPoolListOptions(), token);
    }

    public async Task<CommandResult> Reset(AzBatchPoolResetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Resize(AzBatchPoolResizeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Set(AzBatchPoolSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzBatchPoolShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}