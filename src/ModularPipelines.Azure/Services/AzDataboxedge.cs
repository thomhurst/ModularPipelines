using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzDataboxedge
{
    public AzDataboxedge(
        AzDataboxedgeAlert alert,
        AzDataboxedgeBandwidthSchedule bandwidthSchedule,
        AzDataboxedgeDevice device,
        AzDataboxedgeOrder order,
        ICommand internalCommand
    )
    {
        Alert = alert;
        BandwidthSchedule = bandwidthSchedule;
        Device = device;
        Order = order;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzDataboxedgeAlert Alert { get; }

    public AzDataboxedgeBandwidthSchedule BandwidthSchedule { get; }

    public AzDataboxedgeDevice Device { get; }

    public AzDataboxedgeOrder Order { get; }

    public async Task<CommandResult> ListNode(AzDataboxedgeListNodeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListSku(AzDataboxedgeListSkuOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataboxedgeListSkuOptions(), token);
    }

    public async Task<CommandResult> ShowJob(AzDataboxedgeShowJobOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataboxedgeShowJobOptions(), token);
    }
}