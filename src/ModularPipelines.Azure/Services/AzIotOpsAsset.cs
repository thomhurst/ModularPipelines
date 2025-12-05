using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "ops")]
public class AzIotOpsAsset
{
    public AzIotOpsAsset(
        AzIotOpsAssetDataPoint dataPoint,
        AzIotOpsAssetEvent @event,
        ICommand internalCommand
    )
    {
        DataPoint = dataPoint;
        Event = @event;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzIotOpsAssetDataPoint DataPoint { get; }

    public AzIotOpsAssetEvent Event { get; }

    public async Task<CommandResult> Create(AzIotOpsAssetCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzIotOpsAssetDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzIotOpsAssetListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzIotOpsAssetListOptions(), token);
    }

    public async Task<CommandResult> Query(AzIotOpsAssetQueryOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzIotOpsAssetQueryOptions(), token);
    }

    public async Task<CommandResult> Show(AzIotOpsAssetShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzIotOpsAssetUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}