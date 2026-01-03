using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzElasticSan
{
    public AzElasticSan(
        AzElasticSanVolume volume,
        AzElasticSanVolumeGroup volumeGroup,
        ICommand internalCommand
    )
    {
        Volume = volume;
        VolumeGroup = volumeGroup;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzElasticSanVolume Volume { get; }

    public AzElasticSanVolumeGroup VolumeGroup { get; }

    public async Task<CommandResult> Create(AzElasticSanCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzElasticSanDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzElasticSanDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzElasticSanListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzElasticSanListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ListSku(AzElasticSanListSkuOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzElasticSanListSkuOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzElasticSanShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzElasticSanShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzElasticSanUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzElasticSanUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzElasticSanWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzElasticSanWaitOptions(), cancellationToken: token);
    }
}