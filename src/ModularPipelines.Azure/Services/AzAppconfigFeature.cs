using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("appconfig")]
public class AzAppconfigFeature
{
    public AzAppconfigFeature(
        AzAppconfigFeatureFilter filter,
        ICommand internalCommand
    )
    {
        Filter = filter;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzAppconfigFeatureFilter Filter { get; }

    public async Task<CommandResult> Delete(AzAppconfigFeatureDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAppconfigFeatureDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Disable(AzAppconfigFeatureDisableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAppconfigFeatureDisableOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Enable(AzAppconfigFeatureEnableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAppconfigFeatureEnableOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzAppconfigFeatureListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAppconfigFeatureListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Lock(AzAppconfigFeatureLockOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAppconfigFeatureLockOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Set(AzAppconfigFeatureSetOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAppconfigFeatureSetOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzAppconfigFeatureShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAppconfigFeatureShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Unlock(AzAppconfigFeatureUnlockOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAppconfigFeatureUnlockOptions(), cancellationToken: token);
    }
}