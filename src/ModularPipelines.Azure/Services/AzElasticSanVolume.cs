using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("elastic-san")]
public class AzElasticSanVolume
{
    public AzElasticSanVolume(
        AzElasticSanVolumeSnapshot snapshot,
        ICommand internalCommand
    )
    {
        Snapshot = snapshot;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzElasticSanVolumeSnapshot Snapshot { get; }

    public async Task<CommandResult> Create(AzElasticSanVolumeCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzElasticSanVolumeDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzElasticSanVolumeDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzElasticSanVolumeListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzElasticSanVolumeShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzElasticSanVolumeShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzElasticSanVolumeUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzElasticSanVolumeUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzElasticSanVolumeWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzElasticSanVolumeWaitOptions(), cancellationToken: token);
    }
}