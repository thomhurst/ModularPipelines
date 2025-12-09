using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("elastic-san")]
public class AzElasticSanVolumeGroup
{
    public AzElasticSanVolumeGroup(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzElasticSanVolumeGroupCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzElasticSanVolumeGroupDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzElasticSanVolumeGroupDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzElasticSanVolumeGroupListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzElasticSanVolumeGroupShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzElasticSanVolumeGroupShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzElasticSanVolumeGroupUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzElasticSanVolumeGroupUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzElasticSanVolumeGroupWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzElasticSanVolumeGroupWaitOptions(), token);
    }
}