using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzSig
{
    public AzSig(
        AzSigCreate create,
        AzSigGalleryApplication galleryApplication,
        AzSigImageDefinition imageDefinition,
        AzSigImageVersion imageVersion,
        AzSigShare share,
        AzSigShowCommunity showCommunity,
        ICommand internalCommand
    )
    {
        CreateCommands = create;
        GalleryApplication = galleryApplication;
        ImageDefinition = imageDefinition;
        ImageVersion = imageVersion;
        Share = share;
        ShowCommunityCommands = showCommunity;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSigCreate CreateCommands { get; }

    public AzSigGalleryApplication GalleryApplication { get; }

    public AzSigImageDefinition ImageDefinition { get; }

    public AzSigImageVersion ImageVersion { get; }

    public AzSigShare Share { get; }

    public AzSigShowCommunity ShowCommunityCommands { get; }

    public async Task<CommandResult> Create(AzSigCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzSigDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSigDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzSigListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSigListOptions(), token);
    }

    public async Task<CommandResult> ListCommunity(AzSigListCommunityOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSigListCommunityOptions(), token);
    }

    public async Task<CommandResult> ListShared(AzSigListSharedOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzSigShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSigShowOptions(), token);
    }

    public async Task<CommandResult> ShowCommunity(AzSigShowCommunityOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSigShowCommunityOptions(), token);
    }

    public async Task<CommandResult> ShowShared(AzSigShowSharedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSigShowSharedOptions(), token);
    }

    public async Task<CommandResult> Update(AzSigUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}