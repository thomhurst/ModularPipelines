using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzSpatialAnchorsAccount
{
    public AzSpatialAnchorsAccount(
        AzSpatialAnchorsAccountKey key,
        ICommand internalCommand
    )
    {
        Key = key;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSpatialAnchorsAccountKey Key { get; }

    public async Task<CommandResult> Create(AzSpatialAnchorsAccountCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzSpatialAnchorsAccountDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpatialAnchorsAccountDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzSpatialAnchorsAccountListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpatialAnchorsAccountListOptions(), token);
    }

    public async Task<CommandResult> Show(AzSpatialAnchorsAccountShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpatialAnchorsAccountShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzSpatialAnchorsAccountUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpatialAnchorsAccountUpdateOptions(), token);
    }
}