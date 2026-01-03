using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("ams")]
public class AzAmsAsset
{
    public AzAmsAsset(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzAmsAssetCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzAmsAssetDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsAssetDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> GetEncryptionKey(AzAmsAssetGetEncryptionKeyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsAssetGetEncryptionKeyOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> GetSasUrls(AzAmsAssetGetSasUrlsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsAssetGetSasUrlsOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzAmsAssetListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> ListStreamingLocators(AzAmsAssetListStreamingLocatorsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsAssetListStreamingLocatorsOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzAmsAssetShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsAssetShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzAmsAssetUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsAssetUpdateOptions(), cancellationToken: token);
    }
}