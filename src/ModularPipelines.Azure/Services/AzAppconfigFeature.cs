using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("appconfig")]
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
        return await _command.ExecuteCommandLineTool(options ?? new AzAppconfigFeatureDeleteOptions(), token);
    }

    public async Task<CommandResult> Disable(AzAppconfigFeatureDisableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAppconfigFeatureDisableOptions(), token);
    }

    public async Task<CommandResult> Enable(AzAppconfigFeatureEnableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAppconfigFeatureEnableOptions(), token);
    }

    public async Task<CommandResult> List(AzAppconfigFeatureListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAppconfigFeatureListOptions(), token);
    }

    public async Task<CommandResult> Lock(AzAppconfigFeatureLockOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAppconfigFeatureLockOptions(), token);
    }

    public async Task<CommandResult> Set(AzAppconfigFeatureSetOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAppconfigFeatureSetOptions(), token);
    }

    public async Task<CommandResult> Show(AzAppconfigFeatureShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAppconfigFeatureShowOptions(), token);
    }

    public async Task<CommandResult> Unlock(AzAppconfigFeatureUnlockOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAppconfigFeatureUnlockOptions(), token);
    }
}