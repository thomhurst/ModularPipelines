using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("acr")]
public class AzAcrManifest
{
    public AzAcrManifest(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Delete(AzAcrManifestDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAcrManifestDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzAcrManifestListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAcrManifestListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ListDeleted(AzAcrManifestListDeletedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAcrManifestListDeletedOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ListDeletedTags(AzAcrManifestListDeletedTagsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAcrManifestListDeletedTagsOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ListMetadata(AzAcrManifestListMetadataOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAcrManifestListMetadataOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ListReferrers(AzAcrManifestListReferrersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAcrManifestListReferrersOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Restore(AzAcrManifestRestoreOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAcrManifestRestoreOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzAcrManifestShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAcrManifestShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ShowMetadata(AzAcrManifestShowMetadataOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAcrManifestShowMetadataOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> UpdateMetadata(AzAcrManifestUpdateMetadataOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAcrManifestUpdateMetadataOptions(), cancellationToken: token);
    }
}