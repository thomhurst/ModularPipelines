using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzEdgeorder
{
    public AzEdgeorder(
        AzEdgeorderAddress address,
        AzEdgeorderOrder order,
        AzEdgeorderOrderItem orderItem,
        ICommand internalCommand
    )
    {
        Address = address;
        Order = order;
        OrderItem = orderItem;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzEdgeorderAddress Address { get; }

    public AzEdgeorderOrder Order { get; }

    public AzEdgeorderOrderItem OrderItem { get; }

    public async Task<CommandResult> ListConfig(AzEdgeorderListConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListFamily(AzEdgeorderListFamilyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListMetadata(AzEdgeorderListMetadataOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEdgeorderListMetadataOptions(), token);
    }
}