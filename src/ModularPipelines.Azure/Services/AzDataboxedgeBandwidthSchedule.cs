using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("databoxedge")]
public class AzDataboxedgeBandwidthSchedule
{
    public AzDataboxedgeBandwidthSchedule(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzDataboxedgeBandwidthScheduleCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzDataboxedgeBandwidthScheduleDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataboxedgeBandwidthScheduleDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzDataboxedgeBandwidthScheduleListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzDataboxedgeBandwidthScheduleShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataboxedgeBandwidthScheduleShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzDataboxedgeBandwidthScheduleUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzDataboxedgeBandwidthScheduleWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataboxedgeBandwidthScheduleWaitOptions(), token);
    }
}